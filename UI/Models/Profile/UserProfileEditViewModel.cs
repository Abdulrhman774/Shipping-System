using System.ComponentModel.DataAnnotations;

namespace UI.Models.Profile
{
    public class UserProfileEditViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Second name is required.")]
        [StringLength(100, ErrorMessage = "Second name must not exceed 100 characters.")]
        public string SecondName { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Third name must not exceed 100 characters.")]
        public string? ThirdName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public enGender Gender { get; set; }

        [Url(ErrorMessage = "Invalid image URL.")]
        public string? ImageUrl { get; set; }

        // لرفع ملف الصورة
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? PhoneNumber { get; set; }
    }
}