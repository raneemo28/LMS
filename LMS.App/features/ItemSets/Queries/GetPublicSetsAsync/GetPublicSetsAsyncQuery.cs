using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.ItemSets.Queries.GetPublicSetsAsync;

public record GetPublicSetsAsyncQuery(string? UserId, List<string>? UserRoles) : IRequest<IEnumerable<ItemSet>>;

