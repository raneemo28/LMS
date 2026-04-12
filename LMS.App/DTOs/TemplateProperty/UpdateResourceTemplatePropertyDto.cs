namespace LMS.App.DTOs.ResourceTemplate;

public record UpdateResourceTemplatePropertyDto(
    int PropertyId,
    bool IsRequired,
    int DisplayOrder,
    string? AlternateLabel
);
