using MediatR;

namespace LMS.Application.Features.Item.Commands.UpdateItem;

public record UpdateItemCommand(
    int Id, 
    int? TemplateId,     
    string UserId 
) : IRequest<bool>;