using DAL.Context;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Seeding
{
    public static class IdentitySeeder
    {
        private static readonly string seedAdminEmail = "admin@gmail.com";

        public static async Task SeedAsync(
            ShippingDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedAdminAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER" });
        }

        private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var admin = await userManager.FindByEmailAsync(seedAdminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = seedAdminEmail,
                    Email = seedAdminEmail,
                    EmailConfirmed = true,

                    // ✔ Custom fields
                    FullName = "System Administrator",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    Gender = enGender.Male,
                    ImageUrl = "https://default-image.com/admin.png",

                    PhoneNumber = "+201234567890"
                };


                var result = await userManager.CreateAsync(admin, "Admin@12345");

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", result.Errors.Select(e => e.Description))
                    );
                }

                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}