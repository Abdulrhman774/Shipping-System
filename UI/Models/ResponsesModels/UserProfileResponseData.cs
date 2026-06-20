namespace UI.Models.ResponsesModels;

public class UserProfileResponseData
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ProfileImage { get; set; }
    public enGender Gender { get; set; }
}