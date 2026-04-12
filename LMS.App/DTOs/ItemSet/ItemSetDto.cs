using System;
using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.ItemSet;

public record ItemSetDto(
    int Id,
    string Type,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy,
    string? OwnerId,
    string Title,
    string? Description,
    bool IsPublic,
    List<ResourceValueDto> Values
);
