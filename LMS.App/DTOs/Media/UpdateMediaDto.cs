using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Media;

public record UpdateMediaDto(
    int Id,
    string Type,
    string? OwnerId,
    int? ItemId,
    string StoragePath,
    string FileName,
    string? MimeType,
    long? FileSize,
    string? AltText,
    List<UpdateResourceValueDto> Values
);
