namespace LMS.App.DTOs.Vocabulary;

public record UpdateVocabularyDto(
    int Id,
    string Prefix,
    string NamespaceUri,
    string Label
);
