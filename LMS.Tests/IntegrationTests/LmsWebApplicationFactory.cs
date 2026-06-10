using LMS.Infra.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LMS.Tests.IntegrationTests;

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<LmsWebApplicationFactory> { }

public class LmsWebApplicationFactory : WebApplicationFactory<Program>
{
    private static bool _initialized = false;
    private static readonly object _lock = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            lock (_lock)
            {
                if (_initialized) return;

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scoped = scope.ServiceProvider;

                scoped.GetRequiredService<LibraryDbContext>().Database.Migrate();
                scoped.GetRequiredService<AppIdentityDbContext>().Database.Migrate();
                scoped.GetRequiredService<LoggingDbContext>().Database.Migrate();

                var roleManager = scoped.GetRequiredService<RoleManager<IdentityRole>>();
                foreach (var role in new[] { "Member", "Librarian", "Admin" })
                {
                    if (!roleManager.RoleExistsAsync(role).Result)
                        roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }

                _initialized = true;
            }
        });
    }
}
