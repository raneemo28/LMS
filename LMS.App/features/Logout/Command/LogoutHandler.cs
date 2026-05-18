using MediatR;

namespace LMS.App.Features.Logout.Command;

public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
{
    public LogoutHandler()
    {
    }

    public Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // JWT is stateless — client-side token deletion is the logout mechanism.
        // Future: add server-side token blocklist or revocation state keyed by request.UserId here.
        return Task.FromResult(true);
    }
}
