using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.ItemSet;

public record UpdateItemSetDto(
    int Id,
    string Title,
    string? Description,
    bool IsPublic,
    List<ResourceValueDto>? Values
);
