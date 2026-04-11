using LMS.infra.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LMS.infra;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<LibraryDbContext>>();

        try
        {
            var libraryContext = services.GetRequiredService<LibraryDbContext>();
            var identityContext = services.GetRequiredService<AppIdentityDbContext>();

            // Apply migrations for both DBs to ensure tables exist
            await libraryContext.Database.MigrateAsync();
            await identityContext.Database.MigrateAsync();

            // Run only the Metadata Seeder
            await DbSeeder.SeedSystemMetadataAsync(libraryContext, logger);
            
            logger.LogInformation("LMS Database Initialization completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database initialization.");
            throw;
        }
    }
}