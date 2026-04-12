using System.Collections.Generic;

namespace LMS.App.DTOs.ResourceTemplate;

public record CreateResourceTemplateDto(
    string Label,
    string? Description,
    List<CreateResourceTemplatePropertyDto> Properties
);
