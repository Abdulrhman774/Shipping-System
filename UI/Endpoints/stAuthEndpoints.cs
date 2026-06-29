namespace UI.Endpoints;

public readonly struct stAuthEndpoints
{
    public const string Base = "Api/Auth";

    public const string Register = $"{Base}/Register";
    public const string Login = $"{Base}/Login";
    public const string RotateRefreshToken = $"{Base}/RotateRefreshToken";
    public const string RefreshAccessToken = $"{Base}/Refresh-AccessToken";
    public const string Logout = $"{Base}/Logout";
    public const string ChangePassword = $"{Base}/ChangePassword";
}