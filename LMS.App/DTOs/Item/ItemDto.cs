using System;
using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Item;

public record ItemDto(
    int Id,
    int TemplateId,
    List<ResourceValueDto> Values
);

