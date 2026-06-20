using BL.Common;
using BL.Contract.IServices;
using BL.DTOs.Auth;
using BL.DTOs.RefreshToken;
using BL.DTOs.User;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("Api/Auth")]
public class AuthController : ControllerBase
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly TokenService _tokenService;  
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(TokenService tokenService, IUserService userService,
        IRefreshTokenService refreshTokenService, IAuthService authService)
    {
        _tokenService = tokenService;
        _userService = userService;
        _refreshTokenService = refreshTokenService;
        _authService = authService;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        var result = await _authService.RegisterAsync(request);

        if (!result.Success)
            return BadRequest("result.Errors");

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var response = await _authService.LoginAsync(dto);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
            return Unauthorized("No refresh token found");

        var stored = await _refreshTokenService.GetTokenAsync(refreshToken);
        if (stored == null || stored.Expires < DateTime.UtcNow)
            return Unauthorized("Invalid or expired refresh token");

        await _refreshTokenService.RevokeTokenAsync(stored.UserId);

        var (claims, user) = await _GetClimsById(stored.UserId);
        var newAccessToken = _tokenService.GenerateAccessToken(claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        await _refreshTokenService.SaveTokenAsync(
            stored.UserId, newRefreshToken, DateTime.UtcNow.AddDays(7));

        Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
    }

    [HttpPost("Refresh-AccessToken")]
    public async Task<IActionResult> RefreshAccessToken()
    {
        if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
        {
            return Unauthorized("No refresh token found");
        }

        // Retrieve the refresh token from the database
        var storedToken = await _refreshTokenService.GetTokenAsync(refreshToken);

        if (storedToken == null || storedToken.CurrentState is not enEntityState.Active || storedToken.Expires < DateTime.UtcNow)
        {
            return Unauthorized("Invalid or expired refresh token");
        }

        // Generate a new access token
        var (claims, user) = await _GetClimsById(storedToken.UserId);

        var newAccessToken = _tokenService.GenerateAccessToken(claims);

        return Ok(new { AccessToken = newAccessToken });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
            await _refreshTokenService.RevokeTokenAsync(userId);

        Response.Cookies.Delete("RefreshToken");
        return NoContent();
    }



    #region Helper Methods
    private async Task<(Claim[], UserDto)> _GetClimsByEmailOrUsername(string UsernameOrEmail)
    {
        var user = await _userService.GetUserByEmailOrUsernameAsync(UsernameOrEmail);
        var roles = await _authService.GetRolesAsync(user.Id.ToString());
        var role = roles.FirstOrDefault() ?? "User";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name,           user.Email),
            new Claim(ClaimTypes.Role,           role)
        };

        return (claims, user);
    }
    private async Task<(Claim[], UserDto)> _GetClimsById(string userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        var roles = await _authService.GetRolesAsync(user.Id.ToString());
        var role = roles.FirstOrDefault() ?? "User";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name,           user.Email),
            new Claim(ClaimTypes.Role,           role)
        };

        return (claims, user);
    }
    #endregion
}
