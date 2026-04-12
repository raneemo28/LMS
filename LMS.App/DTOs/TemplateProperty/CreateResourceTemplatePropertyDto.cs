namespace LMS.App.DTOs.ResourceTemplate;

public record CreateResourceTemplatePropertyDto(
    int PropertyId,
    bool IsRequired,
    int DisplayOrder,
    string? AlternateLabel
);
