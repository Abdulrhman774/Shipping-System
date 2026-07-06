using BL.Contract.IServices.Shipment;
using BL.DTOs.Shipment;
using BL.Services.Shipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiConventionType(typeof(ApiConventions))]
public class TestController(IShipmentService shipmentService) : ControllerBase
{
    private readonly IShipmentService _shipmentService = shipmentService;

    [AllowAnonymous]
    [HttpGet("testPublic")]
    public IActionResult testPublic()
    {
        return Ok("✅ Public endpoint — no token needed.");
    }

    [Authorize]
    [HttpGet("testProtected")]
    public IActionResult testProtected()
    {
        return Ok("✅ JWT is valid — you are authenticated.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok("✅ You have the Admin role.");
    }

    [Authorize(Roles = "User")]
    [HttpGet("user")]
    public IActionResult UserOnly()
    {
        return Ok("✅ You have the User role.");
    }


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
