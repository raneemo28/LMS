using System.Collections.Generic;
using LMS.App.DTOs.Property;

namespace LMS.App.DTOs.ResourceTemplate;

public record ResourceTemplateDto(
    int Id,
    string Label,
    string? Description,
    List<PropertyDto> Properties
);
