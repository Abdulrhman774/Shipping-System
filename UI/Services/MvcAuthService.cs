using BL.Common;
using BL.DTOs.Auth.Requests;
using BL.DTOs.Auth.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using UI.Endpoints;

namespace UI.Services;

public class MvcAuthService(GenericApiClient _apiClient)
{
    public async Task<(ApiResponse<TokenResponseDto> Response, ClaimsPrincipal? Principal)> LoginApiAsync(LoginRequestDto dto)
    {
        var response = await _apiClient.PostAsync<TokenResponseDto>(stAuthEndpoints.Login, dto);

        if (!response.Success || response.Data == null)
            return (response, null);

        var principal = CreatePrincipal(response.Data);

        return (response, principal);
    }



    #region Private methods
    private ClaimsPrincipal CreatePrincipal(TokenResponseDto loginData)
    {
        var identity = new ClaimsIdentity(
            BuildClaims(loginData),
            IdentityConstants.ApplicationScheme);

        return new ClaimsPrincipal(identity);
    }
    private List<Claim> BuildClaims(TokenResponseDto loginData)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var jwtToken = tokenHandler.ReadJsonWebToken(loginData.AccessToken);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, GetClaimValue(jwtToken, ClaimTypes.NameIdentifier)),
            new Claim(ClaimTypes.Name, GetClaimValue(jwtToken, ClaimTypes.Name)),
            new Claim(ClaimTypes.Role, GetClaimValue(jwtToken, ClaimTypes.Role)),

            // Optional custom claims
            new Claim("AccessToken", loginData.AccessToken),
            new Claim("Email", loginData.Email ?? string.Empty),
            new Claim("UserId", loginData.UserId.ToString())
        };

        claims.AddRange(
            jwtToken.Claims.Where(c =>
                c.Type != ClaimTypes.NameIdentifier &&
                c.Type != ClaimTypes.Name &&
                c.Type != ClaimTypes.Role));

        return claims;
    }
    private string GetClaimValue(JsonWebToken token, string claimType)
    {
        return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;
    }
    #endregion




    public Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto dto)
    {
        return _apiClient.PostAsync<RegisterResponseDto>(
            stAuthEndpoints.Register,
            dto);
    }


}