using MediatR;
using LMS.App.DTOs.users;

namespace LMS.App.Features.Users.Queries;

public record GetUserByIdQuery(string Id) : IRequest<UserSummaryDto?>;
