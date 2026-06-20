using System.Collections.Generic;
using MediatR;
using LMS.App.DTOs.users;

namespace LMS.App.Features.Users.Queries;

public record GetAllUsersQuery() : IRequest<List<UserSummaryDto>>;
