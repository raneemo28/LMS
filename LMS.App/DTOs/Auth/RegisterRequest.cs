namespace LMS.App.DTOs.Auth;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    string Password,
    string ConfirmPassword,
    string? PhoneNumber
);