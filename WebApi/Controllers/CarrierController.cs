using BL.Contract.IServices;
using BL.DTOs.Carrier;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/Carrier")]
public class CarrierController : BaseController<ICarrierService, TbCarrier, CarrierDto, CreateCarrierDto, UpdateCarrierDto>
{
    public CarrierController(ICarrierService service) : base(service)
    {
    }
}
