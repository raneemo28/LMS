using LMS.Infra;
using LMS.App;
using LMS.Infra.ServiceStorage;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token. Example: eyJhbGci..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Safe, Scoped DB Initialization
// Skipped during integration tests — TestDatabaseInitializer handles seeding instead
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<DatabaseInitializer>().InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is FluentValidation.ValidationException vex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                title = "Validation Error",
                status = 400,
                message = exception?.Message,
                detail = exception?.StackTrace,
                errors = vex.Errors
                            .GroupBy(e => e.PropertyName)
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

        if (exception is KeyNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { title = "Not Found", status = 404, message = exception.Message });
            return;
        }

        if (exception is FileNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { title = "Not Found", status = 404, message = exception.Message });
            return;
        }

        if (exception is InvalidOperationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { title = "Bad Request", status = 400, message = exception.Message });
            return;
        }

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "An unexpected error occurred during request processing.");

        var isDev = context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment();
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Internal Error",
            status = 500,
            message = isDev ? exception?.Message : "An unexpected error occurred.",
            exceptionType = isDev ? exception?.GetType().FullName : null,
            innerMessage = isDev ? exception?.InnerException?.Message : null
        });
    });
});

// ⚠️ UseHttpsRedirection intentionally removed — running on HTTP locally.
// Add it back when deploying to production with HTTPS.
app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
public partial class Program { }