using BL.Services.Simulation;
using DAL.Context;
using DAL.Seeding;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

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
app.Use(async (context, next) =>
{
    // ✅ إذا كان المستخدم مصادقاً والمسار هو Login أو الصفحة الرئيسية
    if (context.User.Identity?.IsAuthenticated == true)
    {
        var path = context.Request.Path.Value?.ToLower();

        // إذا كان في صفحة Login أو الصفحة الرئيسية
        if (path == "/" || path == "/account/login" || string.IsNullOrEmpty(path))
        {
            context.Response.Redirect("/Admin/Dashboard");
            return;
        }
    }

    await next();
});


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=AdminPanel}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();
