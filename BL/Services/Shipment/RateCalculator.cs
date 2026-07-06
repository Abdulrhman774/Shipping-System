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

    // industrial standard for volumetric weight calculation (cubic centimeters / 5000 = kilograms)
    private const double VolumetricDivisor = 5000d; 

    public RateCalculator(
        IGenericRepository<TbSetting> settingRepository,
        IGenericRepository<TbShippingType> shippingTypeRepository,
        IGenericRepository<TbUserSender> senderRepository,
        IGenericRepository<TbUserReceiver> receiverRepository,
        IGenericRepository<TbUserSubscription> subscriptionRepository)
    {
        _settingRepository = settingRepository;
        _shippingTypeRepository = shippingTypeRepository;
        _senderRepository = senderRepository;
        _receiverRepository = receiverRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<decimal> CalculateShippingRate(CreateShipmentDto dto)
    {
        // If the subscription covers the shipment, return 0 as the shipping rate.
        // Otherwise, fall back to standard rate calculation.
        if (dto.UserSubscriptionId is Guid subscriptionId && subscriptionId != Guid.Empty)
        {
            var coveredByPackage = await TryConsumeFromSubscription(subscriptionId, dto);
            if (coveredByPackage)
                return 0m; // Shipping is covered by the subscription package
        }

        // 2) Standard calculation (without subscription, or subscription has exhausted its quota)
        return await CalculateStandardRate(dto);
    }



    #region Private Helper Methods
    private async Task<decimal> CalculateStandardRate(CreateShipmentDto dto)
    {
        var setting = (await _settingRepository.GetAllAsync()).FirstOrDefault()
            ?? throw new InvalidOperationException("Shipping settings are not configured.");

        if (setting.KiloMeterRate is null || setting.KilooGramRate is null)
            throw new InvalidOperationException(
                "Shipping settings are incomplete (KiloMeterRate or KilooGramRate is not set).");

        var kiloMeterRate = (decimal)setting.KiloMeterRate.Value;
        var kilooGramRate = (decimal)setting.KilooGramRate.Value;

        var shippingType = await _shippingTypeRepository.GetByIdAsync(dto.ShippingTypeId)
            ?? throw new InvalidOperationException("Invalid shipping type.");

        var sender = await _senderRepository.GetByIdAsync(dto.SenderId)
            ?? throw new InvalidOperationException("Sender not found.");

        var receiver = await _receiverRepository.GetByIdAsync(dto.ReceiverId)
            ?? throw new InvalidOperationException("Receiver not found.");

        // الحساب الحجمي بالكامل بالـ double (كل المدخلات double أصلاً)
        // for example, if the package is 50cm x 40cm x 30cm, the volumetric weight is:
        // (50 * 40 * 30) / 5000 = 12 kg
        var volumetricWeight = (dto.Width * dto.Height * dto.Length) / VolumetricDivisor;
        var billableWeight = Math.Max(dto.Weight, volumetricWeight);


        var weightCost = (decimal)billableWeight * kilooGramRate * (decimal)shippingType.ShippingFactor;

        var distanceCost = CalculateDistanceCost(sender.CityId, receiver.CityId, kiloMeterRate);

        var totalRate = weightCost + distanceCost;

        return Math.Round(totalRate, 2);
    }

    private decimal CalculateDistanceCost(Guid senderCityId, Guid receiverCityId, decimal kiloMeterRate)
    {
        // TODO: يحتاج جدول مسافات حقيقي بين المدن (Distance Matrix) لحساب دقيق
        // حاليًا: منطق مبسط بناءً على نفس المدينة / مدينة مختلفة
        if (senderCityId == receiverCityId)
            return kiloMeterRate * 10; // مسافة افتراضية داخل نفس المدينة (10 كم)

        return kiloMeterRate * 150; // مسافة افتراضية بين محافظات مختلفة (150 كم)
    }

    private async Task<bool> TryConsumeFromSubscription(Guid subscriptionId, CreateShipmentDto dto)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
        if (subscription is null)
            return false;

        // لازم تجيب الباقة المرتبطة (PackageId) عشان تعرف الحدود الأصلية
        // (ShipmentCount, TotalWeight, NumberOfKiloMeters) وتقارنها بالمستهلك فعليًا
        // لحد دلوقتي من نفس الاشتراك. محتاج نتفق على آلية تتبع الاستهلاك الأول.

        throw new NotImplementedException(
            "Subscription consumption logic needs package quota tracking details.");
    }

    #endregion
}