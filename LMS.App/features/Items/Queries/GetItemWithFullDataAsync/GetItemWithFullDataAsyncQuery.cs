using LMS.App.DTOs.Item;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Items.Queries.GetItemWithFullDataAsync;

public record GetItemWithFullDataAsyncQuery(int Id) : IRequest<ItemDto?>;

