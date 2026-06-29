namespace UI.Services.Contracts;

public interface ITokenRefreshService
{
    Task<bool> RefreshAccessTokenAsync();
    Task<bool> RotateRefreshTokenAsync();
}
