using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var corsPolicy = builder.Configuration["Cors:PolicyName"];


#region DIC
builder.Host.AddSerilogLogging(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration, builder.Environment);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddValidators();

builder.Services.AddCorsPolicy(corsPolicy!);
builder.Services.AddIdentityConfig();
builder.Services.AddJwtAuth(builder.Configuration);


builder.Services.AddAppAuthorization();
builder.Services.AddAppRateLimiting();
builder.Services.AddSwaggerDocs();

builder.Services.AddControllers();
#endregion


// build the app
var app = builder.Build();


#region  Seeding data 
await SeederExtensions.SeedDatabaseAsync(app, false);
#endregion


#region Middleware pipeline
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    // Use the development error handler
    app.UseExceptionHandler("/error-development");
}
else
{
    // Use the production error handler
    app.UseExceptionHandler("/error");
}


app.UseHttpsRedirection();

app.UseCors(corsPolicy!);

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
