namespace LMS.App.DTOs.ApplicationUser;

public record UpdateApplicationUserDto(
    string Id,
    string FirstName,
    string LastName,
    string? MiddleName,
    bool IsActive,
    string? PhoneNumber
);
