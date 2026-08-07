using DAL.Context;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Seeding
{
    public static class IdentitySeeder
    {
        private static readonly string seedAdminEmail = "admin@gmail.com";
        private static readonly string seedAdminPassword = "Admin@12345";

        // تعريف بيانات المستخدمين بأرقام هواتف فريدة
        private static readonly (string Email, string Password, string Role, string FirstName, string LastName, string PhoneNumber)[] DemoUsers =
        [
            ("opmanager@demo.com", "OpManager@123", "OpManager", "Operations", "Manager", "+201234567891"),
            ("reviewer@demo.com",   "Reviewer@123",   "Reviewer",   "Reviewer",   "User",      "+201234567892"),
            ("operator@demo.com",   "Operator@123",   "Op",         "Operator",   "User",      "+201234567893")
        ];

        public static async Task SeedAsync(
            ShippingDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedAdminAsync(userManager);
            await SeedDemoUsersAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User", "OpManager", "Reviewer", "Op" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    });
                }
            }
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

                    FirstName = "System",
                    SecondName = "Control",
                    ThirdName = "Main",
                    LastName = "Administrator",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    Gender = enGender.Male,
                    ImageUrl = "https://default-image.com/admin.png",

                    PhoneNumber = "+201234567890" // ✅ هذا يبقى فريداً
                };

                var result = await userManager.CreateAsync(admin, seedAdminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", result.Errors.Select(e => e.Description))
                    );
                }

                await userManager.AddToRoleAsync(admin, "Admin");
                await userManager.AddToRoleAsync(admin, "User");
            }
        }

        private static async Task SeedDemoUsersAsync(UserManager<ApplicationUser> userManager)
        {
            foreach (var (email, password, role, firstName, lastName, phoneNumber) in DemoUsers)
            {
                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,

                        FirstName = firstName,
                        SecondName = "Demo",
                        ThirdName = "User",
                        LastName = lastName,
                        DateOfBirth = new DateOnly(1995, 1, 1),
                        Gender = enGender.Male,
                        ImageUrl = $"https://default-image.com/{role.ToLower()}.png",

                        PhoneNumber = phoneNumber // ✅ رقم فريد لكل مستخدم
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            string.Join(", ", result.Errors.Select(e => e.Description))
                        );
                    }

                    await userManager.AddToRoleAsync(user, role);
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}