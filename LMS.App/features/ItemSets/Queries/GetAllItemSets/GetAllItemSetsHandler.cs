using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using AutoMapper;
using LMS.App.DTOs.ItemSet;

namespace LMS.App.Features.ItemSets.Queries.GetAllItemSets;
public class GetAllItemSetsHandler : IRequestHandler<GetAllItemSetsQuery, IEnumerable<ItemSetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllItemSetsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ItemSetDto>> Handle(GetAllItemSetsQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ItemSets.GetAllAsync();
        var itemSets = result.Cast<ItemSet>();
        return _mapper.Map<IEnumerable<ItemSetDto>>(itemSets) ?? Enumerable.Empty<ItemSetDto>();
    }
}
