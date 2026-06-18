namespace LMS.App.DTOs.Resource;

public record ResourceDto(
    int Id,
    string Type,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy,
    string? OwnerId
);