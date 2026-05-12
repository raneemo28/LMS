
using MediatR;

namespace LMS.App.Features.Queries.ItemSets.CheckItemSetOwnership;
public record CheckItemSetOwnershipQuery(int Id, string UserId) : IRequest<bool>;
