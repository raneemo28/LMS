using MediatR;

namespace LMS.Application.Features.Item.Commands.CreateItem;

public record CreateItemCommand(
    int TemplateId, 
    string CurrentUserId
) : IRequest<int>;