using System.Collections.Generic;

namespace LMS.App.DTOs.users;

public record UserOperationResult(bool Success, string Message, IEnumerable<string>? Errors = null);