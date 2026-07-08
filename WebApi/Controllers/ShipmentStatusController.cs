using BL.Contract.IServices;
using BL.DTOs.ShipmentStatus;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/ShipmentStatus")]
public class ShipmentStatusController : BaseController<IShipmentStatusService, TbShipmentStatus, ShipmentStatusDto, CreateShipmentStatusDto, UpdateShipmentStatusDto>
{
    public ShipmentStatusController(IShipmentStatusService service) : base(service)
    {
    }
}
