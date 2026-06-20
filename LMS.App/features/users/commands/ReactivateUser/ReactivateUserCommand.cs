using MediatR;
using LMS.App.DTOs.users;

namespace LMS.App.Features.Users.Commands;

public record ReactivateUserCommand(string UserId) : IRequest<UserOperationResult>;
