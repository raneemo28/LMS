using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Media;

public record CreateMediaDto(
    string Type,
    string? OwnerId,
    int? ItemId,
    string StoragePath,
    string FileName,
    string? MimeType,
    long? FileSize,
    string? AltText,
    List<CreateResourceValueDto> Values
);
