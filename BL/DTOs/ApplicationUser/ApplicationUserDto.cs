using BL.DTOs.Base;
using Domain.Entities;

namespace BL.DTOs.ApplicationUser
{
    public class ApplicationUserDto : BaseDto
    {
        public string FullName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public enGender Gender { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
