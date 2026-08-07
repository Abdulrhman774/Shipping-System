namespace UI.Areas.Admin.Models;

public class AdminProfileViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string AvatarInitials { get; set; } = string.Empty;
    public string JoinDate { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
}

public class ChangePasswordViewModel
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
