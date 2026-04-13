using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Item.Queries.GetItemWithFullDataAsync;

public record GetItemWithFullDataAsyncQuery(int Id) : IRequest<object?>;

