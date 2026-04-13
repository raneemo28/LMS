using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;
namespace LMS.Application.Features.Resources.Queries.GetResourcesByTypeName;
public class GetResourcesByTypeHandler : IRequestHandler<GetResourcesByTypeQuery, IEnumerable<Resource>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetResourcesByTypeHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<Resource>> Handle(GetResourcesByTypeQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Resources.GetResourcesByTypeAsync(request.TypeName);
    }
}