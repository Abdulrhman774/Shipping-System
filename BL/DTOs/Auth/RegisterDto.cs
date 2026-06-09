namespace BL.DTOs.Auth
{
    public class RegisterDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
        public enGender Gender { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}