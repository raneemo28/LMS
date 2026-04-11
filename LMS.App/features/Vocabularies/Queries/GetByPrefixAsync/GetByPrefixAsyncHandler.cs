using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;


namespace LMS.Application.Features.Vocabularies.Queries.GetByPrefix;

public class GetByPrefixHandler : IRequestHandler<GetByPrefixQuery, List<Vocabulary>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByPrefixHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Vocabulary>> Handle(GetByPrefixQuery request, CancellationToken cancellationToken)
    {

        var vocabularies = await _unitOfWork.Vocabularies.FindAsync(v => v.Prefix.StartsWith(request.Prefix));
        return vocabularies.Select(v => new Vocabulary
        {
            Id = v.Id,
            Prefix = v.Prefix,
            NamespaceUri = v.NamespaceUri,
            Label = v.Label
        }).ToList();
    }

}