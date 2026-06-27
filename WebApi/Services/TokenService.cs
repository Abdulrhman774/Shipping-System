using BL.Common.Results;
using BL.Contract.IServices;
using BL.DTOs.Auth.Responses;
using BL.DTOs.RefreshToken;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Services;

public class TokenService
{
    private readonly IConfiguration _config;
    private readonly IRefreshTokenService _refreshService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(IConfiguration config, IRefreshTokenService refreshTokenService, UserManager<ApplicationUser> userManager)
    {
        _config = config;
        _refreshService = refreshTokenService;
        _userManager = userManager;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        // Generate a symmetric security key from the secret key in the configuration
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT_SECRET_KEY"]!));

        // Create signing credentials using the security key and HMAC-SHA256 algorithm
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        // Create a JWT token with the specified issuer, audience, claims, expiration time, and signing credentials
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }

    public int GetRefreshTokenExpiryDays()
    {
        return _config.GetValue<int>("Jwt:RefreshTokenExpiryDays");
    }

    public int GetAccessTokenExpiryMinutes()
    {
        return _config.GetValue<int>("Jwt:ExpiryMinutes");
    }

    public async Task<Claim[]> BuildClaimsAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email,           user.Email ?? "Email not found"),
            new Claim(ClaimTypes.Role,           role)
        };
    }


    #region Private Helper Method
    private async Task<Result<(ApplicationUser User, RefreshTokenDto Token)>> ValidateRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Error.Validation(
                "RefreshToken.Required",
                "Refresh token is required.");

        var stored = await _refreshService.GetTokenAsync(refreshToken);

        if (stored is null)
            return Error.Unauthorized(
                "RefreshToken.NotFound",
                "Refresh token not found.");

        if (stored.CurrentState is not enEntityState.Active)
            return Error.Unauthorized(
                "RefreshToken.Revoked",
                "Refresh token has been revoked.");

        if (stored.Expires < DateTime.UtcNow)
            return Error.Unauthorized(
                "RefreshToken.Expired",
                "Refresh token has expired.");

        var user = await _userManager.FindByIdAsync(stored.UserId);

        if (user is null)
            return Error.NotFound(
                "User.NotFound",
                "User no longer exists.");

        return (user, stored);
    }
    
    #endregion


    public async Task<Result<RefreshTokenResponseDto>> RotateRefreshTokenAsync(string refreshToken)
    {
        var validation = await ValidateRefreshTokenAsync(refreshToken);

        if (validation.IsFailure)
            return validation.Errors.ToList();

        var (user, stored) = validation.Value;

        var revoked = await _refreshService.RevokeTokensAsync(stored.UserId);

        if (!revoked)
            return Error.Unexpected(
                "RefreshToken.RevokeFailed",
                "Failed to revoke refresh tokens.");

        var claims = await BuildClaimsAsync(user);

        var accessToken = GenerateAccessToken(claims);
        var newRefreshToken = GenerateRefreshToken();

        var saved = await _refreshService.SaveTokenAsync(
            stored.UserId,
            newRefreshToken,
            DateTime.UtcNow.AddDays(GetRefreshTokenExpiryDays()));

        if (!saved)
            return Error.Unexpected(
                "RefreshToken.SaveFailed",
                "Failed to save refresh token.");

        return new RefreshTokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                GetAccessTokenExpiryMinutes())
        };
    }
    public async Task<Result<RefreshAccessTokenResponseDto>> RefreshAccessTokenAsync(string refreshToken)
    {
        var validation = await ValidateRefreshTokenAsync(refreshToken);

        if (validation.IsFailure)
            return validation.Errors.ToList();

        var (user, _) = validation.Value;

        var claims = await BuildClaimsAsync(user);

        var accessToken = GenerateAccessToken(claims);

        return new RefreshAccessTokenResponseDto
        {
            AccessToken = accessToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                GetAccessTokenExpiryMinutes())
        };
    }
}
