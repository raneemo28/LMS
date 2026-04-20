using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Media;

public record CreateMediaDto(
    int? ItemId,
    string FileName,
    string? AltText,
    int? OwnerId,
    List<ResourceValueDto> Values
);
