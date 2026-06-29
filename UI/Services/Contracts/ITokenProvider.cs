namespace UI.Services.Contracts;

public interface ITokenProvider
{
    string? GetAccessToken();
    void SetAccessToken(string token);
    void RemoveAccessToken();
}
