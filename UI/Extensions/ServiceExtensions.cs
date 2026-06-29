using BL.Contract.IServices;
using BL.Contract.IvwServices;
using BL.Mapping;
using BL.Services;
using BL.Services.vwServices;
using DAL.Context;
using DAL.Contracts;
using DAL.Contracts.IRepositories;
using DAL.Repositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Net.Http.Headers;
using UI.Services;
using UI.Services.Contracts;
using UI.Services.Token;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        #region DbContext
        services.AddDbContext<ShippingDbContext>(options =>
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
        services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));

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

        #region Views-specific services
        services.AddScoped<IVwCitiesCountriesService, VwCitiesCountriesService>();
        #endregion

        #region Mapping services
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        services.AddScoped<BL.Mapping.IMapper, BL.Mapping.AutoMapper>();
        #endregion

        #region Auth - UserManager, SignInManager, etc. services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        #endregion

        #region Entity-specific services
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
        #endregion

        // Add service to can call api endpoint
        services.AddScoped<GenericApiClient>();
        services.AddScoped<MvcAuthService>();

        services.AddScoped<ITokenProvider, SessionTokenProvider>();
        services.AddScoped<ITokenRefreshService, TokenRefreshService>();
        services.AddScoped<IRefreshTokenProvider, CookieRefreshTokenProvider>();


        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        return services;
    }

    public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 9;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
        .AddEntityFrameworkStores<ShippingDbContext>()
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

    public static IServiceCollection AddHttpClientCallingApi(this IServiceCollection services)
    {
        services.AddHttpClient("ApiClient", client =>
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        return services;
    }

    public static IServiceCollection AddSessionServices(this IServiceCollection services)
    {
        // For session storage in memory
        services.AddDistributedMemoryCache(); 

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        // ✅ إضافة HttpContextAccessor للوصول إلى Session من أي مكان
        services.AddHttpContextAccessor();

        return services;
    }
}

