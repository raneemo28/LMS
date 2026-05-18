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
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        LibraryDbContext libraryContext,
        AppIdentityDbContext identityContext,
        RoleManager<IdentityRole> roleManager,
        ILogger<DatabaseInitializer> logger)
    {
        _libraryContext = libraryContext;
        _identityContext = identityContext;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _libraryContext.Database.MigrateAsync();
            await _identityContext.Database.MigrateAsync();
            
            if (!await _roleManager.RoleExistsAsync("Member"))
                await _roleManager.CreateAsync(new IdentityRole("Member"));
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("Librarian"))
                await _roleManager.CreateAsync(new IdentityRole("Librarian"));
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DB init failed.");
            throw;
        }
    }
}
