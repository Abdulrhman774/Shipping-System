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
    Task<Result> UpdateShipment(Guid id, UpdateShipmentRequestDto dto);

    /// <summary>
    /// Transitions a shipment from Created or Returned → Approved.
    /// Administrative action (Reviewer / Admin / Op-Manager).
    /// </summary>
    Task<Result> ApproveShipmentAsync(Guid shipmentId, string? note = null,
        CancellationToken ct = default);

    /// <summary>
    /// Transitions a shipment from Approved or Created → ReadyForShip.
    /// Administrative action (Op / Op-Manager / Admin).
    /// </summary>
    Task<Result> MarkReadyForShipAsync(Guid shipmentId, string? note = null,
        CancellationToken ct = default);

    /// <summary>
    /// Transitions a shipment from ReadyForShip or Approved → Shipped.
    /// Requires a carrier assignment.
    /// Administrative action (Op / Op-Manager / Admin).
    /// </summary>
    Task<Result> MarkShippedAsync(Guid shipmentId, ShipShipmentDto dto,
        CancellationToken ct = default);
}
