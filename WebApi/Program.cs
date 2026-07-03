
using BL.Common;
using DAL.Context;
using DAL.Exceptions;
using DAL.Seeding;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var corsPolicy = builder.Configuration["Cors:PolicyName"]?? "ShippingAPICorsPolicy";


#region DIC
builder.Host.AddSerilogLogging(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration, builder.Environment);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddValidators();

builder.Services.AddCorsPolicy(corsPolicy);
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
app.UseCors(corsPolicy);

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

// Custome middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        ApiResponse response;

        switch (exception)
        {
            case DataAccessException ex:

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                response = ApiResponse.InternalServerError(ex.Message);

                break;

            default:

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                response = ApiResponse.InternalServerError(
                    "An unexpected error occurred.");

                break;
        }

        await context.Response.WriteAsJsonAsync(response);
    });
});


#endregion

app.Run();
