using LMS.Domain.Entities;
using LMS.Domain.Interfaces;

using MediatR;
using AutoMapper;
using LMS.App.DTOs.ItemSet;
using LMS.App.DTOs.Item;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.ItemSets.Queries.GetItemSetWithMembers;
public class GetItemSetWithMembersHandler : IRequestHandler<GetItemSetWithMembersQuery, ItemSetMembersDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    public GetItemSetWithMembersHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<ItemSetMembersDto?> Handle(GetItemSetWithMembersQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ItemSets.GetSetWithMembersAsync(request.Id);
    
        if (result == null) return null;

        var setInfo = _mapper.Map<ItemSetDto>(result.SetInfo) ?? throw new InvalidOperationException(_localizer["UnableToMapItemSetInfo"]);
        var members = _mapper.Map<List<ItemDto>>(result.Members) ?? new List<ItemDto>();
        return new ItemSetMembersDto(setInfo, members);
    }
}
