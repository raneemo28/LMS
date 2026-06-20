namespace LMS.Logging.Microservice.Models;

public record LogRequest(
    string Method,
    string Path,
    int StatusCode,
    double ElapsedMilliseconds,
    string? IpAddress
);
