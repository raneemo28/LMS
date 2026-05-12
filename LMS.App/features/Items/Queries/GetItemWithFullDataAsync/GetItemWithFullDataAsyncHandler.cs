using LMS.Domain.Interfaces;
using MediatR;
using LMS.App.DTOs.Item;
using AutoMapper;    
namespace LMS.App.Features.Items.Queries.GetItemWithFullDataAsync;
public class GetItemWithFullDataAsyncHandler : IRequestHandler<GetItemWithFullDataAsyncQuery, ItemDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetItemWithFullDataAsyncHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ItemDto?> Handle(GetItemWithFullDataAsyncQuery request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Items.GetItemWithFullDataAsync(request.Id);
        if (item == null) return null;
        var itemDto = _mapper.Map<ItemDto>(item);
        return itemDto;
    }
}
