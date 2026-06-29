using UI.Services.Contracts;

namespace UI.Services.Token;

public class CookieRefreshTokenProvider : IRefreshTokenProvider
{
    private const string RefreshTokenKey = "RefreshToken";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieRefreshTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetRefreshToken()
    {
        return _httpContextAccessor.HttpContext?
            .Request
            .Cookies[RefreshTokenKey];
    }

    public void SetRefreshToken(string token, DateTime expiresAt)
    {
        _httpContextAccessor.HttpContext?
            .Response
            .Cookies
            .Append(
                RefreshTokenKey,
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = expiresAt
                });
    }

    public void RemoveRefreshToken()
    {
        _httpContextAccessor.HttpContext?
            .Response
            .Cookies
            .Delete(RefreshTokenKey);
    }
}
