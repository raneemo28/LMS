using System.Collections.Generic;
using LMS.App.DTOs.Property;

namespace LMS.App.DTOs.Vocabulary;

public record UpdateVocabularyDto(
    int Id,
    string Prefix,
    string NamespaceUri,
    string Label,
    List<CreatePropertyDto>? Properties = null
);
