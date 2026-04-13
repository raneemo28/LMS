using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Item.Queries.GetItemWithFullDataAsync;
public class GetItemWithFullDataAsyncHandler : IRequestHandler<GetItemWithFullDataAsyncQuery, object?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetItemWithFullDataAsyncHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ItemDto?> Handle(GetItemWithFullDataAsyncQuery request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Items.GetItemWithFullDataAsync(request.Id);
         return _mapper.Map<ItemDto>(item);
    }
}