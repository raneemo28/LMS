
using MediatR;

namespace LMS.Application.Features.Queries.ItemSets.CheckItemSetOwnership;
public record CheckItemSetOwnershipQuery(int Id, string UserId) : IRequest<bool>;
