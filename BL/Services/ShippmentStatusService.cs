using Domain.Entities;
using BL.DTOs.ShippmentStatus;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class ShippmentStatusService 
    : BaseService<TbShippmentStatus, ShippmentStatusDto, CreateShippmentStatusDto, UpdateShippmentStatusDto>, IShippmentStatusService
{
    public ShippmentStatusService(IGenericRepository<TbShippmentStatus> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}
