using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection; 
using AutoMapper;
using LMS.App.DTOs.Logging;
using LMS.Domain.Entities; 
using LMS.Domain.Interfaces;

namespace LMS.API.middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IMapper mapper, ILogRepository logRepository)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var logDTO = new LogDTO(
                Method: context.Request.Method,
                Path: context.Request.Path,
                StatusCode: context.Response.StatusCode,
                ElapsedMilliseconds: stopwatch.Elapsed.TotalMilliseconds,
                IpAddress: context.Connection.RemoteIpAddress?.ToString()
            );

            var serviceProvider = context.RequestServices;

            _ = Task.Run(async () =>
            {
                try
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var backgroundMapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                        var backgroundRepo = scope.ServiceProvider.GetRequiredService<ILogRepository>();

                        LogEntry logEntry = backgroundMapper.Map<LogEntry>(logDTO);
                        await backgroundRepo.SaveLogAsync(logEntry);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Logging DB background write failed: {ex.Message}");
                }
            });
        }
    }
}