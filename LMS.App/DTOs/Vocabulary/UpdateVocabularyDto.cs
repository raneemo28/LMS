namespace LMS.App.DTOs.Vocabulary;

public record UpdateVocabularyDto(
    int Id,
    string NamespaceUri,
    string Label,
    List<PropertyDto> Properties
);
