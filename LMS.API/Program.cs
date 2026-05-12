using LMS.Infra;
using LMS.App;
using LMS.Infra.ServiceStorage;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(); // ✅ Fixed name
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔒 Safe, Scoped DB Initialization
using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<DatabaseInitializer>().InitializeAsync();

if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is FluentValidation.ValidationException vex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new {
                title = "Validation Error",
                status = 400,
                errors = vex.Errors.GroupBy(e => e.PropertyName)
                                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            });
            return;
        }

        if (exception is UnauthorizedAccessException)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { title = "Forbidden", status = 403, message = "Access denied." });
            return;
        }

        // Log the unexpected error
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "An unexpected error occurred during request processing.");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { title = "Internal Error", status = 500, message = "An unexpected error occurred." });
    });
});

app.UseHttpsRedirection();
app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication(); // ✅ MUST precede Authorization
app.UseAuthorization();  // ✅ Correct order

app.MapControllers();
app.Run();
