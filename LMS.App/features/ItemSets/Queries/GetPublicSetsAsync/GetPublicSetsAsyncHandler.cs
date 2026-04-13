using LMS.Application.Features.ItemSets.Queries.GetAllItemSets;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.ItemSets.Queries.GetPublicSetsAsync;
public class GetPublicSetsAsyncHandler : IRequestHandler<GetPublicSetsAsyncQuery, IEnumerable<ItemSet>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPublicSetsAsyncHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ItemSet>> Handle(GetPublicSetsAsyncQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ItemSets.GetAllAsync();
        var itemSets = result.Cast<ItemSet>().AsQueryable();

        itemSets = itemSets.Where(x => x.IsPublic || x.OwnerId == request.UserId);

        return itemSets.ToList();
    }
}