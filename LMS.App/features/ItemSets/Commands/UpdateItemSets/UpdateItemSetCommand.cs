using MediatR;

namespace LMS.Application.Features.ItemSets.Commands.UpdateItemSets;
public record UpdateItemSetCommand(
    int Id, 
    string Title, 
    string Description, 
    bool IsPublic,
    string UserId, 
    List<string> UserRoles
) : IRequest<bool>;