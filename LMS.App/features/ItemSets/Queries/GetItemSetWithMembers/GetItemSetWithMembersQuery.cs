using LMS.Domain.Interfaces;
using MediatR;
using LMS.App.DTOs.ItemSet;

namespace LMS.App.Features.ItemSets.Queries.GetItemSetWithMembers;

public record GetItemSetWithMembersQuery(int Id) : IRequest<ItemSetMembersDto?>;

