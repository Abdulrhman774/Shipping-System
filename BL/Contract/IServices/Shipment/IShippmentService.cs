using Domain.Entities;
using BL.DTOs.Shipment;
using BL.Common.Results;
using BL.DTOs.UserSender;
using BL.DTOs.UserReceiver;

namespace BL.Contract.IServices.Shipment;

public interface IShipmentService 
    : IBaseService<TbShipment, ShipmentDto, CreateShipmentDto, UpdateShipmentDto>
{
    Task<Result<ShipmentDto>> CreateShipment(CreateShipmentRequestDto requestDto);
    //Task<Result<Guid>> CreateShipment(CreateShipmentDto dto);
    Task<Result<ShipmentDto>> GetShipmentByTrackingNumberAsync(string trackingNumber);
    Task<Result<IEnumerable<ShipmentDto>>> GetShipmentsForUserAsync(string userId);
}
