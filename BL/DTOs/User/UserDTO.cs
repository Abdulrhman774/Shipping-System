namespace BL.DTOs.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
    public enGender Gender { get; set; }
    public string ImageUrl { get; set; } = null!;
    
    public byte Age
    {
        get
        {
            if (DateOfBirth is null)
                return 0;

            var today = DateOnly.FromDateTime(DateTime.Today);

            var age = today.Year - DateOfBirth.Value.Year;

            if (today < DateOfBirth.Value.AddYears(age))
                age--;

            return (byte)age;
        }
    }
}
