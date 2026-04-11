using MediatR;

namespace LMS.Application.Features.ItemSets.Commands.DeleteItemSets;

public record DeleteItemSetCommand(int Id, string UserId, List<string> UserRoles) : IRequest<bool>;