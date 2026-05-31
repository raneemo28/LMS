using LMS.App.DTOs.Auth;
using MediatR;

namespace LMS.App.Features.Register.Commands;

public record RegisterCommand(string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    string Password,
    string ConfirmPassword,
    string? PhoneNumber) : IRequest<AuthResponse>;
