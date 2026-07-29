using BL.Common.Results;
using BL.DTOs.Shipment;
using Domain.Entities;

namespace BL.Contract.IServices.Shipment;

public interface IRateCalculator
{
    public Task<Result<decimal>> CalculateStandardRateAsync(CreateShipmentDto dto, TbShippingType shippingType, decimal distance);


    Task<Result<TbUserSubscription>> TryConsumeFromSubscriptionAsync(
        Guid subscriptionId, CreateShipmentDto dto, decimal distance);

    Task<Result> ApplySubscriptionUsageAsync(
        TbUserSubscription subscription, double weight, decimal distance);
}