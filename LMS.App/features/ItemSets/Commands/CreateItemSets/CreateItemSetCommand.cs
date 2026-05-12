using MediatR;
using LMS.App.DTOs.Value;
namespace LMS.App.Features.ItemSets.Commands.CreateItemSets;

public record CreateItemSetCommand(
    string Title, 
    string? Description, 
    bool IsPublic,
    string OwnerId,
    string? CreatedBy,
    List<ResourceValueDto>? Values
) : IRequest<int>;
