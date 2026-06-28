using BL.Contract.IServices;
using BL.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Extensions;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("Api/User")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllUsersAsync();

        return result.ToActionResult(this);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _userService.GetByIdAsync(id);

        return result.ToActionResult(this);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Search")]
    public async Task<IActionResult> GetByEmailOrUsername([FromQuery] string emailOrUsername)
    {
        var result = await _userService.GetUserByEmailOrUsernameAsync(emailOrUsername);

        return result.ToActionResult(this);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] UpdateUserDto dto)
    {
        var result = await _userService.UpdateAsync(id, dto);

        return result.ToActionResult(this);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromBody] string id)
    {
        var result = await _userService.DeleteAccountAsync(id);

        return result.ToActionResult(this);
    }

    [Authorize]
    [HttpDelete("Me")]
    public async Task<IActionResult> DeleteMyAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var result = await _userService.DeleteAccountAsync(userId);

        return result.ToActionResult(this);
    }



    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("✅ Public endpoint — no token needed.");
    }

    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected()
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

}