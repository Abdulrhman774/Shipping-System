using DAL.Context;
using DAL.Seeding;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Extensions;

public static class SeederExtensions
{
    public static async Task SeedDatabaseAsync(WebApplication app, bool shouldSeed)
    {
        if (!shouldSeed)
            return;

        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ShippingDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await IdentitySeeder.SeedAsync(context, userManager, roleManager);
    }
}