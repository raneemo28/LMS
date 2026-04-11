using MediatR;
using LMS.Domain.Interfaces;
namespace LMS.App.features.Queries.Resources.Queries.GetResourceType;
public class GetResourceTypeHandler : IRequestHandler<GetResourceTypeQuery, object>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetResourceTypeHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<object> Handle(GetResourceTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Resources.GetResourceTypeAsync(request.ResourceId);

        if (result == null)
            throw new KeyNotFoundException("Resource or its Type not found.");

        return result;
    }
}