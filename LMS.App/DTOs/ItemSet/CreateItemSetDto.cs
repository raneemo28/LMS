using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.ItemSet;

public record CreateItemSetDto(
    string Title,
    string? Description,
    bool IsPublic,
    List<ResourceValueDto> Values
);  
//dto as command
