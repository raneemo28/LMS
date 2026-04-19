namespace LMS.App.DTOs.Property;

public record CreatePropertyDto(
    int VocabularyId,
    string LocalName,
    string Label,
    string TermUri
);