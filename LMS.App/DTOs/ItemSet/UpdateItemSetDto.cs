using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.ItemSet;

public record UpdateItemSetDto(
    int Id,
    string Type,
    string? OwnerId,
    string Title,
    string? Description,
    bool IsPublic,
    List<UpdateResourceValueDto> Values
);
