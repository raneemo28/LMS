using System;
using System.Collections.Generic;

namespace LMS.App.DTO.Auth;
public record RegisterRequest(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string Password,
    string ConfirmPassword,
    string PhoneNumber
);