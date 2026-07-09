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
    private readonly IShipmentService _shipmentService;
    public ShipmentController(IShipmentService service) : base(service) { _shipmentService = service; }

    [HttpPost("CreateShipment")]
    //    [Authorize(Roles = "User")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentDto dto)
    {
        var result = await _shipmentService.CreateShipment(dto);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
