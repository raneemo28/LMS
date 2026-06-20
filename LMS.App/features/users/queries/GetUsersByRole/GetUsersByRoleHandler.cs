using LMS.App.DTOs.users;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.App.Features.Users.Queries;

public class GetUsersByRoleHandler : IRequestHandler<GetUsersByRoleQuery, List<UserSummaryDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUsersByRoleHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserSummaryDto>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(request.Role);
        
        var result = usersInRole.Select(u => new UserSummaryDto(
            u.Id, 
            $"{u.FirstName} {u.LastName}", 
            u.Email ?? string.Empty, 
            u.IsActive, 
            u.CreatedAt, 
            request.Role)).ToList();
            
        return result;
    }
}
