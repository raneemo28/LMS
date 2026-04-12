using System;

namespace LMS.App.DTOs.ApplicationUser;

public record ApplicationUserDto(
    string Id,
    string FirstName,
    string LastName,
    string? MiddleName,
    bool IsActive,
    DateTime CreatedAt,
    string? Email,
    string? PhoneNumber
);
