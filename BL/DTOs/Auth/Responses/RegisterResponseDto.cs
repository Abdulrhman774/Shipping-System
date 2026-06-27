namespace BL.DTOs.Auth.Responses;

public class 
    RegisterResponseDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
}

