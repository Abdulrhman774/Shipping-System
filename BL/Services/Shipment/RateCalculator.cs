// BL/Services/Shipment/RateCalculator.cs

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
    private readonly IGenericRepository<TbShippingType> _shippingTypeRepository;
    private readonly IGenericRepository<TbUserSender> _senderRepository;
    private readonly IGenericRepository<TbUserReceiver> _receiverRepository;
    private readonly IGenericRepository<TbUserSubscription> _subscriptionRepository;
    private readonly IGenericRepository<TbSubscriptionPackage> _packageRepository;
    private readonly IDistanceService _distanceService;

    private const double VolumetricDivisor = 5000d;
    private const decimal SameCityBaseDistance = 10m;
    private const decimal DifferentCityFallbackDistance = 150m;

    public RateCalculator(
        IGenericRepository<TbSetting> settingRepository,
        IGenericRepository<TbShippingType> shippingTypeRepository,
        IGenericRepository<TbUserSender> senderRepository,
        IGenericRepository<TbUserReceiver> receiverRepository,
        IGenericRepository<TbUserSubscription> subscriptionRepository,
        IGenericRepository<TbSubscriptionPackage> packageRepository,
        IDistanceService distanceService)
    {
        _settingRepository = settingRepository;
        _shippingTypeRepository = shippingTypeRepository;
        _senderRepository = senderRepository;
        _receiverRepository = receiverRepository;
        _subscriptionRepository = subscriptionRepository;
        _packageRepository = packageRepository;
        _distanceService = distanceService;
    }

    public async Task<Result<decimal>> CalculateShippingRateAsync(CreateShipmentDto dto)
    {
        try
        {
            // 1. Check if subscription covers this shipment
            if (dto.UserSubscriptionId.HasValue && dto.UserSubscriptionId.Value != Guid.Empty)
            {
                var subscriptionResult = await TryConsumeFromSubscriptionAsync(dto.UserSubscriptionId.Value, dto);
                if (subscriptionResult.IsSuccess && subscriptionResult.Value)
                    return Result<decimal>.Success(0m);
            }

            // 2. Calculate standard rate
            return await CalculateStandardRateAsync(dto);
        }
        catch (Exception ex)
        {
            return Result<decimal>.Failure(
                Error.Unexpected("RateCalculationFailed", $"Failed to calculate rate: {ex.Message}"));
        }
    }

    public async Task<Result<bool>> TryConsumeFromSubscriptionAsync(Guid subscriptionId, CreateShipmentDto dto)
    {
        try
        {
            // 1. Get the user subscription
            var subscription = await _subscriptionRepository
                .GetFirstOrDefaultAsync(s => s.Id == subscriptionId && s.IsActive);

            if (subscription is null)
                return Result<bool>.Failure(Error.NotFound("Subscription.NotFound", "User subscription not found."));

            // 2. Check if subscription is expired
            if (subscription.ExpiryDate.HasValue && subscription.ExpiryDate.Value < DateTime.UtcNow)
                return Result<bool>.Failure(Error.Validation("Subscription.Expired", "Subscription has expired."));

            // 3. Get package details
            var package = await _packageRepository.GetByIdAsync(subscription.PackageId);
            if (package is null)
                return Result<bool>.Failure(Error.NotFound("Package.NotFound", "Subscription package not found."));

            // 4. Check shipment count quota
            if (subscription.UsedShipmentCount >= package.ShipmentCount)
                return Result<bool>.Failure(Error.Validation("Subscription.QuotaExceeded",
                    $"Shipment count quota exceeded ({package.ShipmentCount} shipments allowed)."));

            // 5. Check weight quota
            if (subscription.UsedTotalWeight + dto.Weight > package.TotalWeight)
                return Result<bool>.Failure(Error.Validation("Subscription.WeightExceeded",
                    $"Weight exceeds package limit of {package.TotalWeight} kg."));

            // 6. Check distance quota
            var distance = await _distanceService.GetDistanceBetweenSenderAndReceiverAsync(dto.SenderId, dto.ReceiverId);
            if (subscription.UsedTotalDistance + (double)distance > package.NumberOfKiloMeters)
                return Result<bool>.Failure(Error.Validation("Subscription.DistanceExceeded",
                    $"Distance exceeds package limit of {package.NumberOfKiloMeters} km."));

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(
                Error.Unexpected("SubscriptionCheckFailed", $"Failed to check subscription: {ex.Message}"));
        }
    }

    public async Task<Result> UpdateSubscriptionUsageAsync(Guid subscriptionId, CreateShipmentDto dto, decimal distance)
    {
        try
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
            if (subscription is null)
                return Error.NotFound("Subscription.NotFound", "User subscription not found.");

            subscription.UsedShipmentCount++;
            subscription.UsedTotalWeight += dto.Weight;
            subscription.UsedTotalDistance += (double)distance;

            var updated = await _subscriptionRepository.UpdateAsync(subscriptionId, subscription);
            if (!updated)
                return Error.Unexpected("Subscription.UpdateFailed", "Failed to update subscription usage.");

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Error.Unexpected("Subscription.UpdateFailed", $"Failed to update subscription: {ex.Message}");
        }
    }

    #region Private Helper Methods

    private async Task<Result<decimal>> CalculateStandardRateAsync(CreateShipmentDto dto)
    {
        // 1. Get settings
        var settings = await _settingRepository.GetAllAsync();
        var setting = settings.FirstOrDefault();

        if (setting is null)
            return Result<decimal>.Failure(Error.NotFound("Settings.NotFound", "Shipping settings are not configured."));

        if (!setting.KiloMeterRate.HasValue || !setting.KilooGramRate.HasValue)
            return Result<decimal>.Failure(Error.Validation("Settings.Incomplete",
                "Shipping settings are incomplete (KiloMeterRate or KilooGramRate is not set)."));

        var kiloMeterRate = (decimal)setting.KiloMeterRate.Value;
        var kilooGramRate = (decimal)setting.KilooGramRate.Value;

        // 2. Get shipping type
        var shippingType = await _shippingTypeRepository.GetByIdAsync(dto.ShippingTypeId);
        if (shippingType is null)
            return Result<decimal>.Failure(Error.NotFound("ShippingType.NotFound",
                $"Shipping type with ID '{dto.ShippingTypeId}' not found."));

        // 3. Get sender and receiver
        var sender = await _senderRepository.GetByIdAsync(dto.SenderId);
        if (sender is null)
            return Result<decimal>.Failure(Error.NotFound("Sender.NotFound",
                $"Sender with ID '{dto.SenderId}' not found."));

        var receiver = await _receiverRepository.GetByIdAsync(dto.ReceiverId);
        if (receiver is null)
            return Result<decimal>.Failure(Error.NotFound("Receiver.NotFound",
                $"Receiver with ID '{dto.ReceiverId}' not found."));

        // 4. Calculate volumetric weight
        var volumetricWeight = (dto.Width * dto.Height * dto.Length) / VolumetricDivisor;
        var billableWeight = Math.Max(dto.Weight, volumetricWeight);

        // 5. Calculate weight cost
        var weightCost = (decimal)billableWeight * kilooGramRate * (decimal)shippingType.ShippingFactor;

        // 6. Calculate distance cost
        var distance = await _distanceService.GetDistanceBetweenCitiesAsync(sender.CityId, receiver.CityId);
        var distanceCost = kiloMeterRate * distance;

        // 7. Calculate total rate
        var totalRate = weightCost + distanceCost;

        return Result<decimal>.Success(Math.Round(totalRate, 2));
    }

    #endregion
}