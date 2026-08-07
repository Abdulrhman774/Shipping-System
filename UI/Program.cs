using DAL.Context;
using DAL.Seeding;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using UI.Helpers;

var builder = WebApplication.CreateBuilder(args);

#region DIC
// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Host.AddSerilogLogging(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration, builder.Environment);

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddValidators();
builder.Services.AddIdentityConfig();
builder.Services.AddHttpClientCallingApi();
builder.Services.AddSessionServices();

// Simulate current user service for demonstration purposes
//builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
#endregion

var app = builder.Build();

// Enabled seeding data 
bool WantedToSeed = false;

if (WantedToSeed)
{
    // Seed the database with initial data
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ShippingDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await IdentitySeeder.SeedAsync(context, userManager, roleManager);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Warning⚠: must be done before UseAuthentication()
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// ============================================
// ✅ Middleware التوجيه - مع التحقق من Role
// ============================================
app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        var path = context.Request.Path.Value?.ToLower();
        if (path == "/" || path == "/account/login" || string.IsNullOrEmpty(path))
        {
            if (context.User.IsInRole(AppRoles.Admin) ||
                context.User.IsInRole(AppRoles.OpManager) ||
                context.User.IsInRole(AppRoles.Reviewer) ||
                context.User.IsInRole(AppRoles.Op))
            {
                context.Response.Redirect("/Admin/Dashboard");
            }
            else
            {
                context.Response.Redirect("/Home/Index");
            }
            return;
        }
    }
    await next();
});

// ============================================
// ✅ Routes
// ============================================


/*
//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//// Route مخصص للـ Account
//app.MapControllerRoute(
//    name: "account",
//    pattern: "account/{action=Login}/{id?}",
//    defaults: new { controller = "Account" });

//// Route مخصص للـ Home
//app.MapControllerRoute(
//    name: "home",
//    pattern: "home/{action=Index}/{id?}",
//    defaults: new { controller = "Home" });

//app.MapControllerRoute(
//    name: "shipment_create",
//    pattern: "Shipment/Create",
//    defaults: new { controller = "Shipment", action = "Create" });

//app.MapControllerRoute(
//    name: "shipment_update",
//    pattern: "Shipment/Update/{id}",
//    defaults: new { controller = "Shipment", action = "Update" });

//app.MapControllerRoute(
//    name: "shipment_history",
//    pattern: "Shipment/History",
//    defaults: new { controller = "Shipment", action = "History" });

//app.MapControllerRoute(
//    name: "shipment_details",
//    pattern: "Shipment/Details/{id}",
//    defaults: new { controller = "Shipment", action = "Details" });

//app.MapControllerRoute(
//    name: "shipment_confirmation",
//    pattern: "Shipment/Confirmation/{id}",
//    defaults: new { controller = "Shipment", action = "Confirmation" });
*/



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();