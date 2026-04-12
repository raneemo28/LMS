using System;
using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Item;

public record ItemDto(
    int Id,
    string Type,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy,
    string? OwnerId,
    int? TemplateId,
    List<ResourceValueDto> Values
);
