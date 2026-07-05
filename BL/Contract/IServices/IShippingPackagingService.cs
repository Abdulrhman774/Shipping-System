using BL.DTOs.ShippingPackaging;
using Domain.Entities;

namespace BL.Contract.IServices;

public interface IShippingPackagingService
    : IBaseService<TbShippingPackaging, ShippingPackagingDto, CreateShippingPackagingDto, UpdateShippingPackagingDto>
{ }

