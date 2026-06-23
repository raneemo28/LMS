using System.Diagnostics;
using System.Security.Claims;
using LMS.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LMS.API.middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly IHostApplicationLifetime _lifetime;

    public LoggingMiddleware(
        RequestDelegate next,
        IServiceScopeFactory scopeFactory,
        ILogger<LoggingMiddleware> logger,
        IHostApplicationLifetime lifetime)
    {
        _next         = next;
        _scopeFactory = scopeFactory;
        _logger       = logger;
        _lifetime     = lifetime;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.ToString();

        // Skip noisy infrastructure paths — no value logging these
        if (path.StartsWith("/swagger") ||
            path.StartsWith("/favicon") ||
            path.StartsWith("/health")  ||
            path.StartsWith("/metrics"))
        {
            await _next(context);
            return;
        }

        var stopwatch   = Stopwatch.StartNew();
        var method      = context.Request.Method;
        var queryString = context.Request.QueryString.ToString();

        await _next(context);

        stopwatch.Stop();

        // Capture from HttpContext BEFORE Task.Run — HttpContext is not thread-safe
        var statusCode = context.Response.StatusCode;

        // UseAuthentication() runs before this middleware so context.User is populated
        var user = context.User?.FindFirst(ClaimTypes.Email)?.Value
        ?? context.User?.FindFirst(ClaimTypes.Name)?.Value
        ?? context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? "Anonymous";

        var message = $"{method} {path}{queryString} - {statusCode} ({stopwatch.ElapsedMilliseconds}ms)";

        // Fire-and-forget — HTTP response returns to client immediately
        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var publisher   = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                await publisher.Publish(new LogMessage(message, user), _lifetime.ApplicationStopping);
            }
            catch (OperationCanceledException)
            {
                // App is shutting down cleanly — no action needed
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[LoggingMiddleware] Failed to publish log message for {Method} {Path}",
                    method, path);
            }
        }, _lifetime.ApplicationStopping);
    }
}