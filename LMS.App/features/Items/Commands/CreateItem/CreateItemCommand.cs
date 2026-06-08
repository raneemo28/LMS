using LMS.App.DTOs.Item;
using MediatR;

namespace LMS.App.Features.Items.Commands.CreateItem;

public record CreateItemCommand(
    CreateItemDto Dto, 
    string OwnerId
) : IRequest<int>;
