namespace BL.DTOs.User;

public class UpdateUserDto
{
    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public enGender Gender { get; set; }

    public string? ImageUrl { get; set; }

    public string? PhoneNumber { get; set; }
}
