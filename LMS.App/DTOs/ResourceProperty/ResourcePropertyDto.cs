namespace LMS.App.DTOs.ResourceProperty;
public record ResourcePropertyDto(
    int TemplateId,
    int PropertyId,
    bool IsRequired,
    int DisplayOrder,
    string? AlternateLabel
);