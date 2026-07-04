using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public string? ThirdName { get; set; }
        public string LastName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public enGender Gender { get; set; }
        public string ImageUrl { get; set; } = null!;

        public virtual ICollection<TbRefreshToken> RefreshTokens { get; set; }
            = new HashSet<TbRefreshToken>();
    }
}
