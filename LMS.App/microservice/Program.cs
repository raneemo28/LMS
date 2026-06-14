using LMS.Logging.Microservice.Data;
using LMS.Logging.Microservice.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register the logging DB context
builder.Services.AddDbContext<LoggingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LoggingConnection")));

var app = builder.Build();

// Auto-apply EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<LoggingDbContext>().Database.Migrate();
}

// POST /api/logs  — called by the LMS.API LoggingMiddleware (fire-and-forget)
app.MapPost("/api/logs", async (LogRequest request, LoggingDbContext db) =>
{
    var entry = new LogEntry
    {
        Method             = request.Method,
        Path               = request.Path,
        StatusCode         = request.StatusCode,
        ElapsedMilliseconds = request.ElapsedMilliseconds,
        IpAddress          = request.IpAddress,
        CreatedAt          = DateTime.UtcNow
    };

    db.Logs.Add(entry);
    await db.SaveChangesAsync();

    return Results.Created($"/api/logs/{entry.Id}", entry.Id);
});

app.Run();
