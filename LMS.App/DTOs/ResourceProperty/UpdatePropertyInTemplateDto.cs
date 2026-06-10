namespace LMS.App.DTOs.ResourceProperty;

public record UpdatePropertyInTemplateDto(
    bool IsRequired,
    int DisplayOrder,
    string? AlternateLabel
);