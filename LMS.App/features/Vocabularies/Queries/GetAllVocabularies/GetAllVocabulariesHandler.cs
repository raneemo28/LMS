using AutoMapper;
using LMS.App.DTOs.Vocabulary;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Vocabularies.Queries.GetAllVocabularies;

public class GetAllVocabulariesHandler : IRequestHandler<GetAllVocabulariesQuery, List<VocabularyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetAllVocabulariesHandler(IUnitOfWork unitOfWork, IMapper mapper) { _unitOfWork = unitOfWork; _mapper = mapper; }

    public async Task<List<VocabularyDto>> Handle(GetAllVocabulariesQuery request, CancellationToken ct)
    {
        var vocabs = await _unitOfWork.Vocabularies.GetAllAsync();
        return _mapper.Map<List<VocabularyDto>>(vocabs);
    }
}
