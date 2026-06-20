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

    public TokenService(IConfiguration config)
    {
        _config = config;
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

    public int RefreshTokenExpiryMinutes()
    {
        return _config.GetValue<int>("Jwt:RefreshTokenExpiryDays");
    }

    public int GetAccessTokenExpiryMinutes()
    {
        return _config.GetValue<int>("Jwt:ExpiryMinutes");
    }
}
