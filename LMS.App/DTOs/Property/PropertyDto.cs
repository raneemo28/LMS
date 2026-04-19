namespace LMS.App.DTOs.Property;

public record PropertyDto(
    int Id,
    int VocabularyId,
    string LocalName,
    string Label,
    string TermUri
);