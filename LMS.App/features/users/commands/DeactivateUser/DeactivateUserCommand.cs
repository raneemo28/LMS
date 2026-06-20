using MediatR;
using LMS.App.DTOs.users;

namespace LMS.App.Features.Users.Commands;

public record DeactivateUserCommand(string UserId) : IRequest<UserOperationResult>;
