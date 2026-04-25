using LMS.App.DTO.Auth;
using LMS.App.DTOs.Auth;
using MediatR;

namespace LMS.App.features.Login.Command;

public record LoginCommand(LogInRequest Data) : IRequest<AuthResponse>;
