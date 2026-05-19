using LMS.Infra;
using LMS.Infra.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Tests.Infrastructure;

public class LmsApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove SQL Server DbContext registrations
            RemoveAll<DbContextOptions<LibraryDbContext>>(services);
            RemoveAll<DbContextOptions<AppIdentityDbContext>>(services);
            RemoveAll<LibraryDbContext>(services);
            RemoveAll<AppIdentityDbContext>(services);

            // Remove real DatabaseInitializer, replace with test-safe version
            RemoveAll<DatabaseInitializer>(services);
            services.AddScoped<DatabaseInitializer, TestDatabaseInitializer>();

            // Register InMemory databases with service provider caching disabled
            var libraryDbName  = $"TestLibraryDb_{Guid.NewGuid()}";
            var identityDbName = $"TestIdentityDb_{Guid.NewGuid()}";

            services.AddDbContext<LibraryDbContext>(opts =>
                opts.UseInMemoryDatabase(libraryDbName)
                    .EnableServiceProviderCaching(false));

            services.AddDbContext<AppIdentityDbContext>(opts =>
                opts.UseInMemoryDatabase(identityDbName)
                    .EnableServiceProviderCaching(false));
        });

        // Seed the InMemory databases AFTER the host is built
        builder.Configure(app =>
        {
            using var scope = app.ApplicationServices.CreateScope();
            scope.ServiceProvider
                .GetRequiredService<DatabaseInitializer>()
                .InitializeAsync()
                .GetAwaiter()
                .GetResult();
        });
    }

    private static void RemoveAll<T>(IServiceCollection services)
    {
        var toRemove = services.Where(d => d.ServiceType == typeof(T)).ToList();
        foreach (var d in toRemove)
            services.Remove(d);
    }
}