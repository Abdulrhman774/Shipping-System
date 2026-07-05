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
public class TestController : ControllerBase
{
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


    [HttpGet("CreateShipment")]
    [Authorize(Roles = "User")]
    public IActionResult CreateShipment()
    {
        
        return Ok();
    }

}
