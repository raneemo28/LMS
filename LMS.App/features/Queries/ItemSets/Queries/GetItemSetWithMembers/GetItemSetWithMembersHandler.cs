using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.ItemSets.Queries.GetItemSetWithMembers;
public class GetItemSetWithMembersHandler : IRequestHandler<GetItemSetWithMembersQuery, object?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetItemSetWithMembersHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<object?> Handle(GetItemSetWithMembersQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ItemSets.GetSetWithMembersAsync(request.Id);
    }
}