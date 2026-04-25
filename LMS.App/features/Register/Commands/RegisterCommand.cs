using LMS.App.DTO.Auth;
using MediatR;

namespace LMS.App.features.Register.Commands;

public record RegisterCommand(RegisterRequest Data) : IRequest<AuthResponse>;
