using Domain.Entities;
using BL.DTOs.Shipment;
using BL.Common.Results;

namespace BL.Contract.IServices.Shipment;

public interface IShipmentService 
    : IBaseService<TbShipment, ShipmentDto, CreateShipmentDto, UpdateShipmentDto>
{
    Task<Result<Guid>> CreateShipment(CreateShipmentDto dto);
    Task<Result<ShipmentDto>> GetShipmentByTrackingNumberAsync(string trackingNumber);
    Task<Result<IEnumerable<ShipmentDto>>> GetShipmentsForUserAsync(string userId);
}
