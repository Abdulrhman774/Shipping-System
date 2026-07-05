using BL.Contract.IServices;
using BL.DTOs.ShippingPackaging;
using BL.DTOs.ShippingType;
using BL.Mapping;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services;




public class ShippingPackagingService
    : BaseService<TbShippingPackaging, ShippingPackagingDto, CreateShippingPackagingDto, UpdateShippingPackagingDto>, IShippingPackagingService
{
    public ShippingPackagingService(IGenericRepository<TbShippingPackaging> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}
