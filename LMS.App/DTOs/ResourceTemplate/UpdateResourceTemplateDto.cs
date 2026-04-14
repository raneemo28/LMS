using System.Collections.Generic;

namespace LMS.App.DTOs.ResourceTemplate;

public record UpdateResourceTemplateDto(
    int Id,
    string Label,
    string? Description,
    List<PropertyDto> Properties
);
