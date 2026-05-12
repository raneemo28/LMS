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
        // LibraryConnection: For Metadata (Vocabulary, Resource, etc.)
        // IdentityConnection: For Security (Users, Roles, Claims)
        
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("LibraryConnection"),
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

        // 3. JWT Authentication Configuration
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] 
            ?? throw new InvalidOperationException("JWT Key is missing in appsettings.json"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        services.AddAuthorization();
        // 4. Storage Services (MISSING IN SCAN - ADDED NOW)
        services.AddScoped<IMediaProcessingService, MediaProcessingService>();
        services.AddScoped<IMediaStorageService>(sp => 
            new LocalMediaStorageService(sp.GetRequiredService<IConfiguration>()));
        services.AddScoped<IJwtService, JwtService>();

        // 5. Repositories & UnitOfWork (MISSING IN SCAN - ADDED NOW)
        services.AddScoped(typeof(IResourceRepository<>), typeof(ResourceRepository<>));
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemSetRepository, ItemSetRepository>();
        services.AddScoped<IVocabularyRepository, VocabularyRepository>();
        services.AddScoped<IResourceTemplateRepository, ResourceTemplateRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<DatabaseInitializer>(); // Required for Program.cs

        return services;
    }
}
