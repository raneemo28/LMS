namespace LMS.App.DTOs.Vocabulary;

public record PropertyDto(
    int Id,
    int VocabularyId,
    string LocalName,
    string Label,
    string TermUri
);
