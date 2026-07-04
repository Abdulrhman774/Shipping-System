namespace BL.DTOs.Auth.Requests
{
    public class RegisterRequestDto
    {
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public string? ThirdName { get; set; }
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public enGender Gender { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}