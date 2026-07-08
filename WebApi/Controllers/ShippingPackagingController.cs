using BL.Contract.IServices;
using BL.DTOs.ShippingPackaging;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/ShippingPackaging")]
public class ShippingPackagingController : BaseController<IShippingPackagingService, TbShippingPackaging, ShippingPackagingDto, CreateShippingPackagingDto, UpdateShippingPackagingDto>
{
    public ShippingPackagingController(IShippingPackagingService service) : base(service)
    {
    }
}
