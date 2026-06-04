using Domain.Entities;
using BL.DTOs.ShippmentStatus;
using BL.Contracts.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class ShippmentStatusService 
    : BaseService<TbShippmentStatus, ShippmentStatusDto, CreateShippmentStatusDto, UpdateShippmentStatusDto>, IShippmentStatusService
{
    public ShippmentStatusService(IGenericRepository<TbShippmentStatus> repository, IMapper mapper) 
        : base(repository, mapper) { }
}
