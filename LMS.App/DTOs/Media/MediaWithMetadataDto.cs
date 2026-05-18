using System;
using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Media;

public record MediaWithMetadataDto(
    int Id,
    string? FileName,
    string? StoragePath,
    long? FileSize,
    string? MimeType,
    List<MetadataValueDto>? Metadata
);

public record MetadataValueDto(string PropertyLabel, string ValueText);
