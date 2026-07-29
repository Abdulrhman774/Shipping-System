using BL.Common.Results;
using BL.Contract.IServices;
using BL.Contract.IServices.Shipment;
using BL.DTOs.Shipment;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services.Shipment;

public class RateCalculator : IRateCalculator
{
    private readonly IGenericRepository<TbSetting> _settingRepository;
    private readonly IGenericRepository<TbUserSubscription> _subscriptionRepository;
    private readonly IGenericRepository<TbSubscriptionPackage> _packageRepository;

    private const double VolumetricDivisor = 5000d;

    public RateCalculator(
        IGenericRepository<TbSetting> settingRepository,
        IGenericRepository<TbUserSubscription> subscriptionRepository,
        IGenericRepository<TbSubscriptionPackage> packageRepository)
    {
        _settingRepository = settingRepository;
        _subscriptionRepository = subscriptionRepository;
        _packageRepository = packageRepository;
    }

    public async Task<Result<TbUserSubscription>> TryConsumeFromSubscriptionAsync(
        Guid subscriptionId, CreateShipmentDto dto, decimal distance)
    {
        try
        {
            var subscription = await _subscriptionRepository
                .GetFirstOrDefaultAsync(s => s.Id == subscriptionId && s.IsActive);

            if (subscription is null)
                return Error.NotFound("Subscription.NotFound", "User subscription not found.");

            if (subscription.ExpiryDate.HasValue && subscription.ExpiryDate.Value < DateTime.UtcNow)
                return Error.Validation("Subscription.Expired", "Subscription has expired.");

            var package = await _packageRepository.GetByIdAsync(subscription.PackageId);
            if (package is null)
                return Error.NotFound("Package.NotFound", "Subscription package not found.");

            if (subscription.UsedShipmentCount >= package.ShipmentCount)
                return Error.Validation("Subscription.QuotaExceeded",
                    $"Shipment count quota exceeded ({package.ShipmentCount} shipments allowed).");

            if (subscription.UsedTotalWeight + dto.Weight > package.TotalWeight)
                return Error.Validation("Subscription.WeightExceeded",
                    $"Weight exceeds package limit of {package.TotalWeight} kg.");

            if (subscription.UsedTotalDistance + (double)distance > package.NumberOfKiloMeters)
                return Error.Validation("Subscription.DistanceExceeded",
                    $"Distance exceeds package limit of {package.NumberOfKiloMeters} km.");

            return Result<TbUserSubscription>.Success(subscription);
        }
        catch (Exception ex)
        {
            return Error.Unexpected("SubscriptionCheckFailed", $"Failed to check subscription: {ex.Message}");
        }
    }

    public async Task<Result> ApplySubscriptionUsageAsync(
        TbUserSubscription subscription, double weight, decimal distance)
    {
        try
        {
            subscription.UsedShipmentCount++;
            subscription.UsedTotalWeight += weight;
            subscription.UsedTotalDistance += (double)distance;

            var updated = await _subscriptionRepository.UpdateAsync(subscription.Id, subscription);

            return updated
                ? Result.Success()
                : Error.Unexpected("Subscription.UpdateFailed", "Failed to update subscription usage.");
        }
        catch (Exception ex)
        {
            return Error.Unexpected("Subscription.UpdateFailed", $"Failed to update subscription: {ex.Message}");
        }
    }

    public async Task<Result<decimal>> CalculateStandardRateAsync(CreateShipmentDto dto, TbShippingType shippingType, decimal distance)
    {
        try
        {
            var setting = await _settingRepository.GetFirstOrDefaultAsync(_ => true);

            if (setting is null)
                return Error.NotFound("Settings.NotFound", "Shipping settings are not configured.");

            if (!setting.KiloMeterRate.HasValue || !setting.KilooGramRate.HasValue)
                return Error.Validation("Settings.Incomplete",
                    "Shipping settings are incomplete (KiloMeterRate or KilooGramRate is not set).");

            var kiloMeterRate = (decimal)setting.KiloMeterRate.Value;
            var kilooGramRate = (decimal)setting.KilooGramRate.Value;

            var volumetricWeight = (dto.Width * dto.Height * dto.Length) / VolumetricDivisor;
            var billableWeight = Math.Max(dto.Weight, volumetricWeight);

            var weightCost = (decimal)billableWeight * kilooGramRate * (decimal)shippingType.ShippingFactor;
            var distanceCost = kiloMeterRate * distance;

            return Result<decimal>.Success(Math.Round(weightCost + distanceCost, 2));
        }
        catch (Exception ex)
        {
            return Error.Unexpected("RateCalculationFailed", $"Failed to calculate rate: {ex.Message}");
        }
    }
}