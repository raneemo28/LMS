using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.ItemSet;

public record CreateItemSetDto(
    string Type,
    string? OwnerId,
    string Title,
    string? Description,
    bool IsPublic,
    List<CreateResourceValueDto> Values
);
