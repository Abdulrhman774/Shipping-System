namespace UI.Services.Contracts;

public interface IRefreshTokenProvider
{
    string? GetRefreshToken();
    void SetRefreshToken(string token, DateTime expiresAt);
    void RemoveRefreshToken();
}
