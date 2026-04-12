namespace LMS.App.DTOs.Value;

/// <summary>
/// Represents a property value being returned when fetching an Item.
/// The `ResourceId` is omitted because it is already known from the parent Item.
/// </summary>
public record ResourceValueDto(
    int Id,
    int PropertyId,
    string? ValueText,
    string? ValueUri,
    int? ValueResourceId,
    string Type,
    string? Language
);
