namespace LMS.App.DTOs.ResourceProperty;

public record ResourcePropertyDto(
    int PropertyId,
    string LocalName,
    string Label,
    string TermUri,
    bool IsRequired,
    int DisplayOrder,
    string? AlternateLabel
);
