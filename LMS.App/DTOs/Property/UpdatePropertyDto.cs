namespace LMS.App.DTOs.Property;

public record UpdatePropertyDto(
    int Id,
    string LocalName,
    string Label,
    string TermUri
);