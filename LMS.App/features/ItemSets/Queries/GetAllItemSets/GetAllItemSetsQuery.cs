using LMS.Domain.Entities;
using MediatR;
using LMS.App.DTOs.ItemSet;

namespace LMS.Application.Features.ItemSets.Queries.GetAllItemSets;

public record GetAllItemSetsQuery() : IRequest<IEnumerable<ItemSetDto>>;    