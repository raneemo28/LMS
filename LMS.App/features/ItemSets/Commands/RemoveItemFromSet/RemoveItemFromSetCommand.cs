using MediatR;
namespace LMS.Application.Features.ItemSets.Commands.RemoveItemFromSet;
public record RemoveItemFromSetCommand(int SetId, int ItemId, string UserId) : IRequest<bool>;