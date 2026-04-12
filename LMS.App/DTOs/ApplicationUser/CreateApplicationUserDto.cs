namespace LMS.App.DTOs.ApplicationUser;

public record CreateApplicationUserDto(
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    string Password,
    string? PhoneNumber
);
