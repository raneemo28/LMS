using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.ItemSets.Queries.GetPublicSetsAsync;
public class GetPublicSetsAsyncHandler : IRequestHandler<GetPublicSetsAsyncQuery, object?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPublicSetsAsyncHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<object?> Handle(GetPublicSetsAsyncQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ItemSets.GetSetWithMembersAsync(request.Id);
    }
}