namespace LMS.Logging.Microservice.Models;

/// <summary>
/// DTO received by the HTTP endpoint from the middleware.
/// </summary>
public record LogRequest(
    string Method,
    string Path,
    int StatusCode,
    double ElapsedMilliseconds,
    string? IpAddress
);
