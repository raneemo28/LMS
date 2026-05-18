using System.Collections.Generic;
using LMS.App.DTOs.ResourceProperty;

namespace LMS.App.DTOs.ResourceTemplate;

public record ResourceTemplateDto
{
    public int Id { get; init; }
    public string Label { get; init; } = string.Empty;
    public string? Description { get; init; }
    public List<ResourcePropertyDto> Properties { get; init; } = new();
}
