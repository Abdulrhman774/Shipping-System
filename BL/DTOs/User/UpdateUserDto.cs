namespace BL.DTOs.User;

public class UpdateUserDto
{
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public string? ThirdName { get; set; }
    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public enGender Gender { get; set; }

    public string? ImageUrl { get; set; }

    public string? PhoneNumber { get; set; }
}
