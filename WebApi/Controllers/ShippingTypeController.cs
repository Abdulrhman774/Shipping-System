using BL.Contract.IServices;
using BL.DTOs.ShippingType;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/ShippingType")]
public class ShippingTypeController : BaseController<IShippingTypeService, TbShippingType, ShippingTypeDto, CreateShippingTypeDto, UpdateShippingTypeDto>
{
    public ShippingTypeController(IShippingTypeService service) : base(service)
    {
    }
}
