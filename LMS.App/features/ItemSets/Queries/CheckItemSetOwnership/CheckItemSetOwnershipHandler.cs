using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Queries.ItemSets.CheckItemSetOwnership;
public class CheckItemSetOwnershipHandler : IRequestHandler<CheckItemSetOwnershipQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckItemSetOwnershipHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CheckItemSetOwnershipQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ItemSets.IsOwnerAsync(request.Id, request.UserId);
    }
}
