using MediatR;
using LMS.App.DTOs.Value;
namespace LMS.Application.Features.ItemSets.Commands.UpdateItemSets;
public record UpdateItemSetCommand(
    int Id, 
    string Title, 
    string Description, 
    bool IsPublic,
    string UserId, 
    List<string> UserRoles,
    List<ResourceValueDto>? Values
) : IRequest<bool>; 