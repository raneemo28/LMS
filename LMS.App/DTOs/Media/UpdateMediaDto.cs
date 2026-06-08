using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Media;

public record UpdateMediaDto(
    int Id,
    int? ItemId,
    string FileName,
    string? AltText,
    List<ResourceValueDto> Values
);