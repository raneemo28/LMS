using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;
namespace LMS.Application.Features.Resources.Queries.GetResourcesByTypeName;
public class GetResourcesByTypeNameHandler : IRequestHandler<GetResourcesByTypeNameQuery, IEnumerable<Resource>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetResourcesByTypeNameHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<Resource>> Handle(GetResourcesByTypeNameQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Resources.GetResourcesByTypeNameAsync(request.TypeName);
    }
}