using Domain.Entities;
using BL.DTOs.ShippingType;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class ShippingTypeService 
    : BaseService<TbShippingType, ShippingTypeDto, CreateShippingTypeDto, UpdateShippingTypeDto>, IShippingTypeService
{
    public ShippingTypeService(IGenericRepository<TbShippingType> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}