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

    public async Task<object?> Handle(GetItemWithFullDataAsyncQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Items.GetItemWithFullDataAsync(request.Id);
    }
}