using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Item;

/// <summary>
/// This DTO is the main entry point from the frontend when creating a new Item.
/// The frontend first requests the template properties from the backend, 
/// renders a form, and then bundles the user's answers into the <see cref="Values"/> collection.
/// </summary>
public record CreateItemDto(
    string Type,
    string? OwnerId,
    int? TemplateId,
    
    /// <summary>
    /// Contains all the dynamic property values filled out by the user in the frontend form.
    /// The frontend is responsible for correlating the user's input with the correct PropertyId.
    /// </summary>
    List<CreateResourceValueDto> Values
);
