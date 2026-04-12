using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Item;

public record UpdateItemDto(
    int Id,
    string Type,
    string? OwnerId,
    int? TemplateId,
    List<UpdateResourceValueDto> Values
);
