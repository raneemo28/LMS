namespace LMS.App.DTOs.Value;

/// <summary>
/// Represents a single property value filled out by the user during Item creation.
/// Notice lack of ResourceId: When sent to the backend, the parent Item does not yet exist.
/// Entity Framework Core will automatically generate and assign the ResourceId when the parent Item is saved!
/// </summary>
public record CreateResourceValueDto(
    /// <summary>
    /// The ID of the Template's Property (e.g., 14 for "Author"). 
    /// The frontend knows this ID because it fetched the Template before drawing the form.
    /// </summary>
    int PropertyId,
    
    // Explicit breakdown depending on the value type.
    string? ValueText,
    string? ValueUri,
    int? ValueResourceId,
    
    string Type,
    string? Language
);
