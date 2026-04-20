using System.Collections.Generic;
using LMS.App.DTOs.ResourceProperty;

namespace LMS.App.DTOs.ResourceTemplate;

public record ResourceTemplateDto(
    int Id,
    string Label,
    string? Description,
    List<ResourcePropertyDto> Properties
);
