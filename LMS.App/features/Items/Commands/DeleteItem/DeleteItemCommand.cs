using MediatR;

namespace LMS.Application.Features.Item.Commands.DeleteItem;

public record DeleteItemCommand(int Id, string UserId) : IRequest<bool>;