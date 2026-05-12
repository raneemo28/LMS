using System.Collections.Generic;
using LMS.App.DTOs.Property;

namespace LMS.App.DTOs.Vocabulary;

public record VocabularyDto(
    int Id,
    string Prefix,
    string NamespaceUri,
    string Label
)
{
    public List<PropertyDto> Properties { get; set; } = new();
};
