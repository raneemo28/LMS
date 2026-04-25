using LMS.App.DTO.Auth;
using MediatR;

namespace LMS.App.features.Logout.Command;

public record LogoutCommand() : IRequest<bool>;
    