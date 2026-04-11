using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.ItemSets.Queries.GetPublicSetsAsync;

public record GetPublicSetsAsyncQuery(int Id) : IRequest<object?>;

