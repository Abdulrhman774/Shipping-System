using BL.Contract.IServices.Shipment;
using BL.DTOs.Shipment;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/Shipment")]
public class ShipmentController : BaseController<IShipmentService, TbShipment, ShipmentDto, CreateShipmentDto, UpdateShipmentDto>
{
    public ShipmentController(IShipmentService service) : base(service)
    {
    }
}
