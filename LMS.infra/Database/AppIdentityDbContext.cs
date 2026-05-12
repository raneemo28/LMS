using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infra.Database;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) 
        : base(options) 
    { 
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Isolate Identity tables into the 'auth' schema
        builder.HasDefaultSchema("auth");

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.MiddleName).HasMaxLength(50);
        });
    }
}
