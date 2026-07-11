using BL.Common.Results;
using BL.DTOs.Shipment;

namespace BL.Contract.IServices.Shipment;

public interface IRateCalculator
{
    /// <summary>
    /// Calculates the shipping rate for a shipment
    /// </summary>
    Task<Result<decimal>> CalculateShippingRateAsync(CreateShipmentDto dto);

    /// <summary>
    /// Checks if a subscription can cover the shipment
    /// </summary>
    Task<Result<bool>> TryConsumeFromSubscriptionAsync(Guid subscriptionId, CreateShipmentDto dto);

    /// <summary>
    /// Updates subscription usage after shipment creation
    /// </summary>
    Task<Result> UpdateSubscriptionUsageAsync(Guid subscriptionId, CreateShipmentDto dto, decimal distance);
}