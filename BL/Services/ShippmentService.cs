using Domain.Entities;
using BL.DTOs.Shipment;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class ShipmentService 
    : BaseService<TbShipment, ShipmentDto, CreateShipmentDto, UpdateShipmentDto>, IShipmentService
{
    public ShipmentService(IGenericRepository<TbShipment> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}