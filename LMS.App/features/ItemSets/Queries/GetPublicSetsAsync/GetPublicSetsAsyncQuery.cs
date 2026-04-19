using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using LMS.App.DTOs.ItemSet;

namespace LMS.Application.Features.ItemSets.Queries.GetPublicSetsAsync;

public record GetPublicSetsAsyncQuery(string UserId) : IRequest<IEnumerable<ItemSetDto>>;

