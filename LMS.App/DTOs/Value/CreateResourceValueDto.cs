
namespace LMS.App.DTOs.Value;
public record CreateResourceValueDto(
    int PropertyId,
    string? ValueText,
    string? ValueUri,
    int? ValueResourceId,
    string  Type,
    string? Language
);
