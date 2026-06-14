using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace LMS.API.middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoggingMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            // Fire-and-forget: send the log to the isolated logging microservice
            _ = Task.Run(async () =>
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("LoggingService");

                    var payload = new
                    {
                        Method              = context.Request.Method,
                        Path                = context.Request.Path.ToString(),
                        StatusCode          = context.Response.StatusCode,
                        ElapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds,
                        IpAddress           = context.Connection.RemoteIpAddress?.ToString()
                    };

                    var json    = JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    await client.PostAsync("/api/logs", content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Logging microservice call failed: {ex.Message}");
                }
            });
        }
    }
}