using Domain.Entities;
using BL.DTOs.ShipmentStatus;

namespace BL.Contract.IServices;

public interface IShipmentStatusService 
    : IBaseService<TbShipmentStatus, ShipmentStatusDto, CreateShipmentStatusDto, UpdateShipmentStatusDto> { }
