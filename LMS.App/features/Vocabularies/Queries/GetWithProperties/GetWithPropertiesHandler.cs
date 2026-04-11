using LMS.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Application.Features.Vocabularies.Queries.GetWithProperties;

public class GetWithPropertiesHandler : IRequestHandler<GetWithPropertiesQuery, object>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWithPropertiesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<object> Handle(GetWithPropertiesQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Vocabularies.GetWithPropertiesAsync(request.Id);

        if (result == null) 
            throw new KeyNotFoundException("Vocabulary not found.");

        return result;  
    }
}