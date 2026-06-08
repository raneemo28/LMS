namespace LMS.App.DTOs.Value;

public record UpdateResourceValueDto(
    string? ValueText,
    int? ValueResourceId,
    string Type,
    string? Language
);