using MediatR;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
namespace LMS.Application.Features.Resources.Queries.GetResourcesByType;
public class GetResourcesByTypeHandler<T> : IRequestHandler<GetResourcesByTypeQuery<T>, IEnumerable<T>> where T : Resource
{
    private readonly IUnitOfWork _unitOfWork;

    public GetResourcesByTypeHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<T>> Handle(GetResourcesByTypeQuery<T> request, CancellationToken cancellationToken)
    {
        // استدعاء الدالة من مستودع الموارد
        return await _unitOfWork.Resources.GetResourcesByTypeAsync<T>();
    }
}