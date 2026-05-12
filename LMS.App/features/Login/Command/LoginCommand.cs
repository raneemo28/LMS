using LMS.App.DTOs.Auth;
using MediatR;

namespace LMS.App.Features.Login.Command;

public record LoginCommand(LogInRequest Data) : IRequest<AuthResponse>;
