using LMS.App.DTOs.users;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.App.Features.Users.Queries;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserSummaryDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllUsersHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserSummaryDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _userManager.Users.ToList();
        var result = new List<UserSummaryDto>();
        
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserSummaryDto(user.Id, $"{user.FirstName} {user.LastName}", user.Email ?? string.Empty, user.IsActive, user.CreatedAt, roles.FirstOrDefault() ?? "Member"));
        }
        
        return result;
    }
}
