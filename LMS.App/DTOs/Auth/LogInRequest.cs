using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Auth;

public record LogInRequest
(
    string  Email ,
    string Password
);