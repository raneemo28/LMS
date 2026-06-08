using MediatR;
using LMS.App.DTOs.ItemSet;
namespace LMS.App.Features.ItemSets.Commands.CreateItemSets;

public record CreateItemSetCommand(
    CreateItemSetDto Dto,
    string OwnerId
) : IRequest<int>;

