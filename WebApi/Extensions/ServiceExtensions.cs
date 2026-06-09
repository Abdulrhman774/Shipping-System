using BL.Contract;
using BL.Contract.IServices;
using BL.Mapping;
using BL.Services;
using DAL.Context;
using DAL.Contracts;
using DAL.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using WebApi.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        #region DbContext
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            options.UseSqlServer(connectionString);

            if (env.IsDevelopment())
            {
                options.LogTo(Console.WriteLine, LogLevel.Information)
                       .EnableSensitiveDataLogging()
                       .EnableDetailedErrors();
            }
        });
        #endregion

        return services;
    }

    public static IHostBuilder AddSerilogLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        Serilog.Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
                connectionString: configuration.GetConnectionString("DefaultConnection"),
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true
                })
            .CreateLogger();

        hostBuilder.UseSerilog();

        return hostBuilder;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<ICarrierRepository, CarrierRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<ISettingRepository, SettingRepository>();
        services.AddScoped<IShippingTypeRepository, ShippingTypeRepository>();
        services.AddScoped<IShippmentRepository, ShippmentRepository>();
        services.AddScoped<IShippmentStatusRepository, ShippmentStatusRepository>();
        services.AddScoped<ISubscriptionPackageRepository, SubscriptionPackageRepository>();
        services.AddScoped<IUserReceiverRepository, UserReceiverRepository>();
        services.AddScoped<IUserSenderRepository, UserSenderRepository>();
        services.AddScoped<IUserSubscriptionRepository, UserSubscriptionRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // deprecated: using generic base service for all entities, as it may not fit all cases and may cause issues with specific logic for certain entities
        //services.AddScoped(typeof(IBaseService<T, TDto, TCreateDto, TUpdateDto>), typeof(BaseService<T, TDto, TCreateDto, TUpdateDto>));

        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        services.AddScoped<BL.Mapping.IMapper, BL.Mapping.AutoMapper>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ICarrierService, CarrierService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<IShippingTypeService, ShippingTypeService>();
        services.AddScoped<IShippmentService, ShippmentService>();
        services.AddScoped<IShippmentStatusService, ShippmentStatusService>();
        services.AddScoped<ISubscriptionPackageService, SubscriptionPackageService>();
        services.AddScoped<IUserReceiverService, UserReceiverService>();
        services.AddScoped<IUserSenderService, UserSenderService>();
        services.AddScoped<IUserSubscriptionService, UserSubscriptionService>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 9;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();


        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.Cookie.Name = "ShippingAuthCookie";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;
        });



        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
        });



        return services;
    }

}

