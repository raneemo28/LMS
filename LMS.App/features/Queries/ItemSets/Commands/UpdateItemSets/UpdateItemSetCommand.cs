using MediatR;

namespace LMS.Application.Features.ItemSets.Commands.UpdateItemSets;
// في الـ Command record
public record UpdateItemSetCommand(
    int Id, 
    string Title, 
    string Description, 
    bool IsPublic,
    string UserId, 
    List<string> UserRoles
) : IRequest<bool>;