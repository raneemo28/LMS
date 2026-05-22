using System.Reflection;
using System.Text;
using LMS.App.Interface;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infra.Database;
using LMS.Infra.Repository;
using LMS.Infra.ServiceStorage;
using LMS.Infra.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LMS.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Database Contexts
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("LibraryConnection"),
                b => b.MigrationsAssembly("LMS.Infra")));
         services.AddDbContext<LoggingDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("LoggingConection"),
                b => b.MigrationsAssembly("LMS.Infra")));
        services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("IdentityConnection"),
                b => b.MigrationsAssembly("LMS.Infra")));

        // 2. Identity & RBAC Configuration
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

        // 3. JWT Authentication — explicitly override the schemes AddIdentity set
        //    AddIdentity internally registers Cookie as the default scheme.
        //    We must override DefaultAuthenticateScheme AND DefaultChallengeScheme
        //    so that [Authorize] uses JWT and returns 401, not a cookie redirect → 404.
        var jwtSettings = configuration.GetSection("Jwt");
        var jwtKey = jwtSettings["Key"];
        if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
            throw new InvalidOperationException("JWT Key must be set in configuration and must be at least 32 characters long.");

        var key = Encoding.UTF8.GetBytes(jwtKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = jwtSettings["Issuer"],
                ValidAudience            = jwtSettings["Audience"],
                IssuerSigningKey         = new SymmetricSecurityKey(key)
            };
        });

        services.AddAuthorization();

        // 4. Storage Services
        services.AddScoped<IMediaProcessingService, MediaProcessingService>();
        services.AddScoped<IMediaStorageService>(sp =>
            new LocalMediaStorageService(sp.GetRequiredService<IConfiguration>()));
        services.AddScoped<IJwtService, JwtService>();

        // 5. Repositories & UnitOfWork
        services.AddScoped(typeof(IResourceRepository<>), typeof(ResourceRepository<>));
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemSetRepository, ItemSetRepository>();
        services.AddScoped<IVocabularyRepository, VocabularyRepository>();
        services.AddScoped<IResourceTemplateRepository, ResourceTemplateRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<ILogRepository,LogRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}