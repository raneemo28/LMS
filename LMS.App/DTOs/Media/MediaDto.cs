using System;
using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Media;

public record MediaDto(
    int Id,
    string Type,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy,
    string? OwnerId,
    int? ItemId,
    string StoragePath,
    string FileName,
    string? MimeType,
    long? FileSize,
    string? AltText,
    List<ResourceValueDto> Values
);
