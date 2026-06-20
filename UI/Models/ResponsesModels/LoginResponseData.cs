namespace UI.Models.ResponsesModels;

public class LoginResponseData
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string Email { get; set; } = null!;
}
