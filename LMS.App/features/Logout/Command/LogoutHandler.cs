using MediatR;
using Microsoft.AspNetCore.Identity;
using LMS.Domain.Entities;

namespace LMS.App.Features.Logout.Command;

public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutHandler(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // JWT is stateless — the client must discard the token.
        // For cookie-based sessions, sign out here.
        await _signInManager.SignOutAsync();
        // Future: add token to a blocklist keyed by request.UserId here.
        return true;
    }
}
