using System.Collections.Generic;

namespace LMS.App.DTOs.ResourceTemplate;

public record ResourceTemplateDto(
    int Id,
    string Label,
    string? Description,
    List<PropertyDto> Properties
);
