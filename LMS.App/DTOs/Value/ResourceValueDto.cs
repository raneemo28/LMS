namespace LMS.App.DTOs.Value;

public record ResourceValueDto(
    int Id,
    int PropertyId,
    string? ValueText,
    string? ValueUri,
    int? ValueResourceId,
    string Type,
    string? Language
);
