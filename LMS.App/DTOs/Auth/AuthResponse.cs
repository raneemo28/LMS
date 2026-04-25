using System;
using System.Collections.Generic;

namespace LMS.App.DTO.Auth;

public record AuthResponse(
    string Token,
    string FullName,
    string Email,
    string Role,
    bool Success,
    string Message
);