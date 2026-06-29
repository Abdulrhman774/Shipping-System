using UI.Services.Contracts;

namespace UI.Services.Token;

public class SessionTokenProvider : ITokenProvider
{
    private const string AccessTokenKey = "AccessToken";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetAccessToken()
    {
        return _httpContextAccessor.HttpContext?
            .Session
            .GetString(AccessTokenKey);
    }

    public void SetAccessToken(string token)
    {
        _httpContextAccessor.HttpContext?
            .Session
            .SetString(AccessTokenKey, token);
    }

    public void RemoveAccessToken()
    {
        _httpContextAccessor.HttpContext?
            .Session
            .Remove(AccessTokenKey);
    }
}
