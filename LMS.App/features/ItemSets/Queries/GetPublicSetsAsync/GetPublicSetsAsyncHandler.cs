using LMS.App.Features.ItemSets.Queries.GetAllItemSets;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using System.Linq;
using AutoMapper;
using LMS.App.DTOs.ItemSet;

namespace LMS.App.Features.ItemSets.Queries.GetPublicSetsAsync;
public class GetPublicSetsAsyncHandler : IRequestHandler<GetPublicSetsAsyncQuery, IEnumerable<ItemSetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPublicSetsAsyncHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ItemSetDto>> Handle(GetPublicSetsAsyncQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ItemSets.GetAllAsync();
        var itemSets = result.Cast<ItemSet>().AsQueryable();

        itemSets = itemSets.Where(x => x.IsPublic || x.OwnerId == request.UserId);

        return _mapper.Map<IEnumerable<ItemSetDto>>(itemSets);
    }
}
