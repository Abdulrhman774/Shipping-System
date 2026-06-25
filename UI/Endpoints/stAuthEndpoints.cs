namespace UI.Endpoints;

public readonly struct stAuthEndpoints
{
    public const string Base = "Api/Auth";

    public const string Register = $"{Base}/Register";
    public const string Login = $"{Base}/login";
    public const string RefreshToken = $"{Base}/RefreshToken";
    public const string RefreshAccessToken = $"{Base}/Refresh-AccessToken";
    public const string Logout = $"{Base}/logout";
}