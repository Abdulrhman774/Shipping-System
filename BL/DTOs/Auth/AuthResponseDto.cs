namespace BL.DTOs.Auth;

public class 
    AuthResponseDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public enGender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfileImage { get; set; }
    public string Token { get; set; } = null!;
    public bool Success { get; set; }
    //public List<string>? Errors { get; set; }
}