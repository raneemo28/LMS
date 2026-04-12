namespace LMS.App.DTOs.Vocabulary;

public record VocabularyDto(
    int Id,
    string Prefix,
    string NamespaceUri,
    string Label
);
