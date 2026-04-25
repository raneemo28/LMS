using System.Reflection;
using LMS.infra;
using LMS.App;
using LMS.App.Interface;
using LMS.Infrastructure.ServicesStorage;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication2();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var uploadPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot/uploads");
if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
builder.Services.AddScoped<IMediaProcessingService, MediaProcessingService>();


var app = builder.Build();
await app.InitializeDatabaseAsync();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        if (exception is FluentValidation.ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            await context.Response.WriteAsJsonAsync(new { 
                title = "Validation Error", 
                status = 400, 
                errors = errors 
            });
        }
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
