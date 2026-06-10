using MediatR;
using LMS.App.DTOs.Item;

namespace LMS.App.Features.Items.Commands.UpdateItem;

public record UpdateItemCommand(
    UpdateItemDto Dto,     
    string OwnerId
) : IRequest<bool>; 
