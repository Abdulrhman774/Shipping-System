namespace UI.Models.ResponsesModels;

public class RegisterResponseData
{
    public bool Success { get; set; } 
    public string Message { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public List<string>? Errors { get; set; }

}