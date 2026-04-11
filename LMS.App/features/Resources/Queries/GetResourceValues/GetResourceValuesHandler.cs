using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Resources.Queries.GetResourceValues;

public class GetResourceValuesHandler : IRequestHandler<GetResourceValuesQuery, object>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetResourceValuesHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<object> Handle(GetResourceValuesQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Resources.GetResourceValuesAsync(request.ResourceId);

        if (result == null)
            throw new KeyNotFoundException($"Resource with ID {request.ResourceId} not found.");

        return result;
    }
}