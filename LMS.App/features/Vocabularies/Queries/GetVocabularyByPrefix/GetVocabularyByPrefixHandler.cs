using AutoMapper;
using LMS.App.DTOs.Vocabulary;
using LMS.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Application.Features.Vocabularies.Queries.GetVocabularyByPrefix;

public class GetVocabularyByPrefixHandler : IRequestHandler<GetVocabularyByPrefixQuery, List<VocabularyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetVocabularyByPrefixHandler(IUnitOfWork unitOfWork, IMapper mapper) { _unitOfWork = unitOfWork; _mapper = mapper; }

    public async Task<List<VocabularyDto>> Handle(GetVocabularyByPrefixQuery request, CancellationToken ct)
    {
        var vocabs = await _unitOfWork.Vocabularies.FindAsync(v => v.Prefix.StartsWith(request.Prefix));
        return _mapper.Map<List<VocabularyDto>>(vocabs);
    }
}