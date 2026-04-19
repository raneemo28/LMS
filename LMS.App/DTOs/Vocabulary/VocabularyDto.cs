using System.Collections.Generic;
using LMS.App.DTOs.Property;

namespace LMS.App.DTOs.Vocabulary;

public record VocabularyDto(
    int Id,
    string Prefix,
    string NamespaceUri,
    string Label,
    List<PropertyDto> Properties
);