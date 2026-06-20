using MediatR;

namespace LMS.App.Features.Items.Commands.DeleteItem;

public record DeleteItemCommand(int Id, string UserId) : IRequest<bool>;

