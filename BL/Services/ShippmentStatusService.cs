using Domain.Entities;
using BL.DTOs.ShipmentStatus;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class ShipmentStatusService 
    : BaseService<TbShipmentStatus, ShipmentStatusDto, CreateShipmentStatusDto, UpdateShipmentStatusDto>, IShipmentStatusService
{
    public ShipmentStatusService(IGenericRepository<TbShipmentStatus> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}
