using System.Collections.Generic;

namespace LMS.App.DTOs.Vocabulary;

public record VocabularyDto(
    int Id,
    string NamespaceUri,
    string Label,
    List<PropertyDto> Properties
);
