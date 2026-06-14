using LMS.Logging.Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Logging.Microservice.Data;

public class LoggingDbContext : DbContext
{
    public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options) { }

    public DbSet<LogEntry> Logs { get; set; }
}
