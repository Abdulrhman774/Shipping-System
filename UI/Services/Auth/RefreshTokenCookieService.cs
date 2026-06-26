namespace UI.Services.Auth;

public sealed class RefreshTokenCookieService
{
    private const string CookieName = "ShippingRefreshToken";
    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromDays(7);

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RefreshTokenCookieService> _logger;

    public RefreshTokenCookieService(IHttpContextAccessor httpContextAccessor,ILogger<RefreshTokenCookieService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string? GetRefreshToken()
    {
        var value = HttpContext?.Request.Cookies[CookieName];
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public void SetRefreshToken(string refreshToken, DateTime? expiresUtc = null)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            _logger.LogWarning("Attempted to store an empty RefreshToken cookie — skipped.");
            return;
        }

        var options = BuildCookieOptions(expiresUtc ?? DateTime.UtcNow.Add(DefaultExpiry));
        HttpContext?.Response.Cookies.Append(CookieName, refreshToken, options);
    }

    public void DeleteRefreshToken()
    {
        // Overwrite with an expired cookie so the browser removes it immediately.
        var options = BuildCookieOptions(DateTime.UtcNow.AddDays(-1));
        HttpContext?.Response.Cookies.Append(CookieName, string.Empty, options);
    }


    #region Private region
    private HttpContext? HttpContext => _httpContextAccessor.HttpContext;

    private static CookieOptions BuildCookieOptions(DateTime expiresUtc) =>
        new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiresUtc,
        };

    #endregion
}