using Domain.Entities;
using BL.DTOs.Shippment;
using BL.Contracts.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class ShippmentService 
    : BaseService<TbShippment, ShippmentDto, CreateShippmentDto, UpdateShippmentDto>, IShippmentService
{
    public ShippmentService(IGenericRepository<TbShippment> repository, IMapper mapper) 
        : base(repository, mapper) { }
}