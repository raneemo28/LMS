using MediatR;

namespace LMS.App.Features.Logout.Command;

public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
{
    public LogoutHandler()
    {
    }

    public Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
