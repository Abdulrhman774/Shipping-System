using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public enGender Gender { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
