using Domain.Entities;
using BL.DTOs.ShippingType;

namespace BL.Contract.IServices;

public interface IShippingTypeService 
    : IBaseService<TbShippingType, ShippingTypeDto, CreateShippingTypeDto, UpdateShippingTypeDto> { }
