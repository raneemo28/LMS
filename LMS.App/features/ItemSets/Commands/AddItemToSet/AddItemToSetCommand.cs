using MediatR;
namespace LMS.App.Features.ItemSets.Commands.AddItemToSet;
public record AddItemToSetCommand(
    int SetId, 
    int ItemId, 
    string UserId
) : IRequest<bool>;
