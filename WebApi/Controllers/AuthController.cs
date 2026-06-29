using BL.Contract.IServices;
using BL.DTOs.Auth.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebApi.Extensions;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("Api/Auth")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;  
    private readonly IAuthService _authService;

    public AuthController(TokenService tokenService, IAuthService authService)
    {
        _tokenService = tokenService;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    [EnableRateLimiting("AuthLimiter")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);

        return result.ToActionResult(this);
    }

    [AllowAnonymous]
    [EnableRateLimiting("AuthLimiter")]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var response = await _authService.LoginAsync(dto);

        return response.ToActionResult(this);
    }

    [Authorize]
    [HttpPost("RotateRefreshToken")]
    public async Task<IActionResult> RotateRefreshToken(
    [FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _tokenService.RotateRefreshTokenAsync(dto.RefreshToken);

        return result.ToActionResult(this);
    }


    [Authorize]
    [HttpPost("Refresh-AccessToken")]
    public async Task<IActionResult> RefreshAccessToken(
    [FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _tokenService.RefreshAccessTokenAsync(dto.RefreshToken);

        return result.ToActionResult(this);
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout(string userId)
    {
        var result = await _authService.LogoutAsync(userId);

        return result.ToActionResult(this);
    }

    [Authorize]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequstDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var result = await _authService.ChangePasswordAsync(userId, dto);

        return result.ToActionResult(this);
    }
}
