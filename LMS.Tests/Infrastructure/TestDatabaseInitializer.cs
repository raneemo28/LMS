using LMS.Domain.Entities;
using LMS.Infra;
using LMS.Infra.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LMS.Tests.Infrastructure;

public class TestDatabaseInitializer : DatabaseInitializer
{
    private readonly LibraryDbContext _libraryContext;
    private readonly AppIdentityDbContext _identityContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public TestDatabaseInitializer(
        LibraryDbContext libraryContext,
        AppIdentityDbContext identityContext,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ILogger<DatabaseInitializer> logger)
        : base(libraryContext, identityContext, roleManager, userManager, logger)
    {
        _libraryContext  = libraryContext;
        _identityContext = identityContext;
        _roleManager     = roleManager;
        _userManager     = userManager;
    }

    public override async Task InitializeAsync()
    {
        await _libraryContext.Database.EnsureCreatedAsync();
        await _identityContext.Database.EnsureCreatedAsync();

        foreach (var role in new[] { "Member", "Librarian", "Admin" })
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

        const string adminEmail    = "admin@lms.test";
        const string adminPassword = "Admin@12345";

        if (await _userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new ApplicationUser
            {
                UserName       = adminEmail,
                Email          = adminEmail,
                FirstName      = "Test",
                LastName       = "Admin",
                IsActive       = true,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}