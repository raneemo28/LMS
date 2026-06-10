using MediatR;
using LMS.App.DTOs.ItemSet;
namespace LMS.App.Features.ItemSets.Commands.UpdateItemSets;
public record UpdateItemSetCommand(
    UpdateItemSetDto Dto,
    string UserId, 
    List<string> UserRoles
) : IRequest<bool>; 


