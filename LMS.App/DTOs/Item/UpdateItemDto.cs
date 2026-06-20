using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Item;

public record UpdateItemDto(
    int Id,
    int TemplateId,
    List<ResourceValueDto> Values
);