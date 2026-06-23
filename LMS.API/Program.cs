using LMS.Infra;
using LMS.App;
using LMS.Infra.ServiceStorage;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using LMS.API.middlewares;
using MassTransit;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

var builder = WebApplication.CreateBuilder(args);

// 1. Localization
builder.Services.AddLocalization();

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(ErrorMessages));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 2. Infrastructure & Application Services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// 3. MassTransit — publisher only, no consumers on the LMS side
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        cfg.Host(host, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

// 4. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name        = "Authorization",
        Type        = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme      = "Bearer",
        BearerFormat = "JWT",
        In          = Microsoft.OpenApi.Models.ParameterLocation.Header,
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

// 5. Localization Options
var supportedCultures = new[] { "en-US", "ar-SA" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures    = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures  = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};
localizationOptions.RequestCultureProviders.Insert(0,
    new QueryStringRequestCultureProvider { QueryStringKey = "lang", UIQueryStringKey = "lang" });

builder.Services.AddSingleton(localizationOptions);

var app = builder.Build();

// Apply Localization
var localizeOptions = app.Services.GetRequiredService<RequestLocalizationOptions>();
app.UseRequestLocalization(localizeOptions);

// DB Initialization (skipped in Testing environment)
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

// Global Exception Handler
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
                title   = "Validation Error",
                status  = 400,
                message = exception?.Message,
                detail  = exception?.StackTrace,
                errors  = vex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            });
            return;
        }

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "An unexpected error occurred.");

        var isDev = context.RequestServices
            .GetRequiredService<IWebHostEnvironment>().IsDevelopment();

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            title         = "Internal Error",
            status        = 500,
            message       = isDev ? exception?.Message : "An unexpected error occurred.",
            exceptionType = isDev ? exception?.GetType().FullName : null,
            innerMessage  = isDev ? exception?.InnerException?.Message : null
        });
    });
});

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseStaticFiles();

// FIX: Authentication BEFORE LoggingMiddleware so context.User is populated
// when the middleware reads JWT claims to capture the real user identity.
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();
app.Run();

public partial class Program { } // Enables integration testing via InternalsVisibleTo