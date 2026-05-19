// FILE: LMS.Infra/DatabaseInitializer.cs  (REPLACE THE EXISTING FILE)
using LMS.Domain.Entities;
using LMS.Infra.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMS.Infra;

public class DatabaseInitializer
{
    private readonly LibraryDbContext _libraryContext;
    private readonly AppIdentityDbContext _identityContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        LibraryDbContext libraryContext,
        AppIdentityDbContext identityContext,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ILogger<DatabaseInitializer> logger)
    {
        _libraryContext = libraryContext;
        _identityContext = identityContext;
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }

    public virtual async Task InitializeAsync()
    {
        try
        {
            // 1. Apply any pending migrations
            await _libraryContext.Database.MigrateAsync();
            await _identityContext.Database.MigrateAsync();

            // 2. Seed roles
            foreach (var role in new[] { "Member", "Librarian", "Admin" })
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            // 3. Seed the default admin account
            //    Change these credentials before deploying to production!
            const string adminEmail    = "admin@lms.com";
            const string adminPassword = "Admin@12345";   // meets the password policy

            var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin is null)
            {
                var admin = new ApplicationUser
                {
                    UserName  = adminEmail,
                    Email     = adminEmail,
                    FirstName = "System",
                    LastName  = "Admin",
                    IsActive  = true,
                    EmailConfirmed = true   // skip e-mail confirmation for seed account
                };

                var createResult = await _userManager.CreateAsync(admin, adminPassword);
                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    _logger.LogInformation("Default admin seeded: {Email}", adminEmail);
                }
                else
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to seed admin: {Errors}", errors);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DB initialisation failed.");
            throw;
        }
    }
}
