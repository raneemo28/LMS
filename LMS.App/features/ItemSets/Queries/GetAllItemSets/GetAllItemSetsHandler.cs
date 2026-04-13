using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.ItemSets.Queries.GetAllItemSets;
public class GetAllItemSetsHandler : IRequestHandler<GetAllItemSetsQuery, IEnumerable<ItemSet>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllItemSetsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<ItemSet>> Handle(GetAllItemSetsQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ItemSets.GetAllAsync();
        var itemSets = result.Cast<ItemSet>();
        if (itemSets == null) return null!;
        return itemSets;
    }
}