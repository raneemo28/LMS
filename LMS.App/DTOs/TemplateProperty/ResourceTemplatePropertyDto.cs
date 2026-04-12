namespace LMS.App.DTOs.ResourceTemplate;

public record ResourceTemplatePropertyDto(
    int PropertyId,
    bool IsRequired,
    int DisplayOrder,
    string? AlternateLabel
);
