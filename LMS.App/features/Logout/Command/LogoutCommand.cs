using MediatR;

namespace LMS.App.Features.Logout.Command;

public record LogoutCommand(string UserId) : IRequest<bool>;
    
