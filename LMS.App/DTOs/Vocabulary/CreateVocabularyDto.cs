namespace LMS.App.DTOs.Vocabulary;

public record CreateVocabularyDto(
    string Prefix,
    string NamespaceUri,
    string Label
);
