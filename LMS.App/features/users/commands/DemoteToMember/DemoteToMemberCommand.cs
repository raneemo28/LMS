using MediatR;
using LMS.App.DTOs.users;

namespace LMS.App.Features.Users.Commands;

public record DemoteToMemberCommand(string UserId) : IRequest<UserOperationResult>;
