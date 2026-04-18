namespace LMS.App.DTOs.Value;
// also the same for update value Dto
public record ResourceValueDto(
    int Id,
    int PropertyId,
    string? ValueText,
    string? ValueUri,
    int? ValueResourceId,
    string Type,
    string? Language
);
