using Logging.Infrastructure.Contexts;
using Logging.Infrastructure.Repositories;
using Logging.Application.Services;
using Logging.Application.IServices;
using Logging.Domain.IRepositories;
using Logging.API.Consumers;
using Logging.Application.Profiles;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LoggingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(LoggingProfile).Assembly));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LogMessageConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        cfg.Host(host, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();
    db.Database.Migrate();
}

app.UseAuthorization();
app.MapControllers();
app.Run();