namespace LMS.App.DTOs.users;
public record UserSummaryDto(
    string Id,
    string FullName,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    string Role
);