using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.ItemSets.Queries.GetItemSetWithMembers;

public record GetItemSetWithMembersQuery(int Id) : IRequest<object?>;

