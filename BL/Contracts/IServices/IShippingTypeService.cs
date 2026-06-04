using Domain.Entities;
using BL.DTOs.ShippingType;

namespace BL.Contracts.IServices;

public interface IShippingTypeService 
    : IBaseService<TbShippingType, ShippingTypeDto, CreateShippingTypeDto, UpdateShippingTypeDto> { }
