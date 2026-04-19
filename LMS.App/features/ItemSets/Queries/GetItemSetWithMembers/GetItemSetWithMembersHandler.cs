using LMS.Domain.Entities;
using LMS.Domain.Interfaces;

using MediatR;
using AutoMapper;
using LMS.App.DTOs.ItemSet;
using LMS.App.DTOs.Item;

namespace LMS.Application.Features.ItemSets.Queries.GetItemSetWithMembers;
public class GetItemSetWithMembersHandler : IRequestHandler<GetItemSetWithMembersQuery, ItemSetMembersDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetItemSetWithMembersHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ItemSetMembersDto?> Handle(GetItemSetWithMembersQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ItemSets.GetSetWithMembersAsync(request.Id);
    
        if (result == null) return null;
        return new ItemSetMembersDto(
            _mapper.Map<ItemSetDto>(result.SetInfo),
            _mapper.Map<List<ItemDto>>(result.Members)
        );  
    }
}