using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Auth;

public record CustomRegisterRequest
(
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    string Password,
    string ConfirmPassword,
    string? PhoneNumber
);