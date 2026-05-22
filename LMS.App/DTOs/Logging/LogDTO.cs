namespace LMS.App.DTOs.Logging;

public record LogDTO(
    string Method,
    string Path,
    int StatusCode,
    double ElapsedMilliseconds,
    string? IpAddress
);