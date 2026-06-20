using LMS.App.DTOs.users;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.App.Features.Users.Queries;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserSummaryDto?>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserSummaryDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        if (user is null) return null;
        
        var roles = await _userManager.GetRolesAsync(user);
        return new UserSummaryDto(
            user.Id, 
            $"{user.FirstName} {user.LastName}", 
            user.Email ?? string.Empty, 
            user.IsActive, 
            user.CreatedAt, 
            roles.FirstOrDefault() ?? "Member");
    }
}
