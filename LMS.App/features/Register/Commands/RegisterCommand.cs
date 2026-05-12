using LMS.App.DTOs.Auth;
using MediatR;

namespace LMS.App.Features.Register.Commands;

public record RegisterCommand(RegisterRequest Data) : IRequest<AuthResponse>;
