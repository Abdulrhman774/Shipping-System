
using DAL.Context;
using DAL.Seeding;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

#region DIC
builder.Host.AddSerilogLogging(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration, builder.Environment);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddValidators();

builder.Services.AddIdentityConfig();

builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddAppAuthorization();
builder.Services.AddAppRateLimiting();
builder.Services.AddSwaggerDocs();

builder.Services.AddControllers();
#endregion


var app = builder.Build();


#region  Seeding data 
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
#endregion


#region Middleware pipeline
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
