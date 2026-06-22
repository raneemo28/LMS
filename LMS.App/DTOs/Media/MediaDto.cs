using System;
using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Media;

public record MediaDto(
    int Id,
    int? ItemId,
    string FileName,
    string? AltText,
    string? MimeType,   
    long? FileSize,   
    List<ResourceValueDto> Values
);
