using MediatR;
using System.Collections.Generic;

namespace LMS.Application.Features.Item.Commands.UpdateItem;

public record UpdateItemCommand(
    int Id, 
    string Title, 
    string? Description, 
    bool IsPublic,
    int? TemplateId,     
    string UserId,      
    List<string> UserRoles 
) : IRequest<bool>;