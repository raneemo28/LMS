using LMS.App.DTOs.Auth;
using MediatR;

namespace LMS.App.Features.Login.Command;

public record LoginCommand(string Email ,string Password) : IRequest<AuthResponse>;
