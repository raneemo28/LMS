using MediatR;

namespace LMS.Application.Features.ItemSets.Commands.CreateItemSets;
public record CreateItemSetCommand(string Title, string Description, bool IsPublic) : IRequest<bool>;