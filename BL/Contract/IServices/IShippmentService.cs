using Domain.Entities;
using BL.DTOs.Shipment;

namespace BL.Contract.IServices;

public interface IShipmentService 
    : IBaseService<TbShipment, ShipmentDto, CreateShipmentDto, UpdateShipmentDto> { }
