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
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Serilog.Sinks.MSSqlServer;
    using System.Text;
    using System.Threading.RateLimiting;
    using WebApi.Services;


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
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddSingleton<TokenService>();

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

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
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

            return services;
        }

        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),

                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

            //services.AddSingleton<IAuthorizationHandler, StudentOwnerOrAdminHandler>();

            return services;
        }

        public static IServiceCollection AddAppRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("AuthLimiter", httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ip,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });
            });

            return services;
        }

        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shipping API",
                    Version = "v1"
                });

                var jwtScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                };

                options.AddSecurityDefinition("Bearer", jwtScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });

            return services;
        }

    }

