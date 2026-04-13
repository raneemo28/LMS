using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Item;


public record CreateItemDto(
    int? TemplateId,
    List<ResourceValueDto> Values
);
