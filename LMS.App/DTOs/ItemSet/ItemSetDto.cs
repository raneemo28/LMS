using System;
using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.ItemSet;

public record ItemSetDto(
    int Id,
    string Title,
    string? Description,
    bool IsPublic,
    List<ResourceValueDto>? Values
);
