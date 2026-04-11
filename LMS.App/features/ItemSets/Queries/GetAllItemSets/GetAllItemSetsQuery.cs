using LMS.Domain.Entities;
using MediatR;

namespace LMS.Application.Features.ItemSets.Queries.GetAllItemSets;

public record GetAllItemSetsQuery(string? UserId, List<string>? UserRoles) : IRequest<IEnumerable<ItemSet>>;