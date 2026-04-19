using AutoMapper;
using LMS.App.DTOs.Vocabulary;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Queries.GetVocabularyById;

public class GetVocabularyByIdHandler : IRequestHandler<GetVocabularyByIdQuery, VocabularyDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetVocabularyByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) { _unitOfWork = unitOfWork; _mapper = mapper; }

    public async Task<VocabularyDto?> Handle(GetVocabularyByIdQuery request, CancellationToken ct)
    {
        var vocab = await _unitOfWork.Vocabularies.GetByIdAsync(request.Id);
        if (vocab == null) return null;

        var props = await _unitOfWork.Vocabularies.GetPropertiesByVocabularyIdAsync(request.Id);
        var dto = _mapper.Map<VocabularyDto>(vocab);
        dto.Properties = _mapper.Map<List<LMS.App.DTOs.Property.PropertyDto>>(props);
        return dto;
    }
}