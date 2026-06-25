using BL.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using UI.Endpoints;

namespace UI.Services;

public class AuthenticationService(GenericApiClient apiClient)
{
    public async Task<(bool Success, ClaimsPrincipal? Principal, string? Error, TokenResponseDto? Data)> LoginAsync(LoginDto dto)
    {

        var response = await apiClient.PostAsync<TokenResponseDto>(stAuthEndpoints.Login, dto);

        if (!response.Success || response.Data == null)
            return (false, null, response.Error ?? "Login failed", null);

        var loginData = response.Data;
        var claims = BuildClaimsFromToken(loginData);

        // ✅ استخدم IdentityConstants.ApplicationScheme بدلاً من CookieAuthenticationDefaults.AuthenticationScheme
        var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
        var principal = new ClaimsPrincipal(identity);

        return (true, principal, null, loginData);
    }

    private List<Claim> BuildClaimsFromToken(TokenResponseDto loginData)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var jwtToken = tokenHandler.ReadJsonWebToken(loginData.AccessToken);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, GetClaimValue(jwtToken, ClaimTypes.NameIdentifier)),
            new Claim(ClaimTypes.Name, GetClaimValue(jwtToken, ClaimTypes.Name)),
            new Claim(ClaimTypes.Role, GetClaimValue(jwtToken, ClaimTypes.Role)),
            new Claim("AccessToken", loginData.AccessToken),
            new Claim("Email", loginData.Email ?? ""),
            new Claim("UserId", loginData.UserId.ToString())
        };

        var otherClaims = jwtToken.Claims
            .Where(c => c.Type != ClaimTypes.NameIdentifier &&
                       c.Type != ClaimTypes.Name &&
                       c.Type != ClaimTypes.Role);
        claims.AddRange(otherClaims);

        return claims;
    }

    private string GetClaimValue(JsonWebToken token, string claimType)
    {
        return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;
    }
}