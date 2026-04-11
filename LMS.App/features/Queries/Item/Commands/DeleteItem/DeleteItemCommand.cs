using MediatR;

namespace LMS.Application.Features.Item.Commands.DeleteItem;

public record DeleteItemCommand(int Id, string UserId, List<string> UserRoles) : IRequest<bool>;