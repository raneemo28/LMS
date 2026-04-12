namespace LMS.App.DTOs.Value;

/// <summary>
/// Represents a property value being submitted during an Item update.
/// If `Id` is provided, the backend updates the existing value.
/// If `Id` is null or 0, the backend knows this is a brand new value being added.
/// As with creation, `ResourceId` is omitted because it is inherited from the parent Item.
/// </summary>
public record UpdateResourceValueDto(
    int? Id, 
    int PropertyId,
    string? ValueText,
    string? ValueUri,
    int? ValueResourceId,
    string Type,
    string? Language
);
