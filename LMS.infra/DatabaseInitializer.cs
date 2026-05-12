using LMS.Infra.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LMS.Infra;
public class DatabaseInitializer
{
    private readonly IServiceProvider _services;
    public DatabaseInitializer(IServiceProvider services) => _services = services;

    public async Task InitializeAsync()
    {
        using var scope = _services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<LibraryDbContext>>();
        try
        {
            await scope.ServiceProvider.GetRequiredService<LibraryDbContext>().Database.MigrateAsync();
            await scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>().Database.MigrateAsync();
            
            // Seed Identity Roles
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync("Member"))
                await roleManager.CreateAsync(new IdentityRole("Member"));
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            
            await DbSeeder.SeedSystemMetadataAsync(
                scope.ServiceProvider.GetRequiredService<LibraryDbContext>(), logger);
        }
        catch (Exception ex) { logger.LogError(ex, "DB init failed."); throw; }
    }
}
