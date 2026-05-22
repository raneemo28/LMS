using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infra.Database;

public class LoggingDbContext : DbContext
{
    public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options) { }

    public DbSet<LogEntry> Logs { get; set; }
}