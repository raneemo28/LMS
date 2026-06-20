using AutoMapper;
using LMS.App.DTOs.Property;
using LMS.App.DTOs.Vocabulary;
using LMS.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.App.Features.Vocabularies.Queries.GetAllVocabularies;

public class GetAllVocabulariesHandler : IRequestHandler<GetAllVocabulariesQuery, List<VocabularyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllVocabulariesHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    { 
        _unitOfWork = unitOfWork; 
        _mapper = mapper; 
    }

    public async Task<List<VocabularyDto>> Handle(GetAllVocabulariesQuery request, CancellationToken ct)
    {
        var vocabs = await _unitOfWork.Vocabularies.GetAllAsync();
        
        var allProperties = await _unitOfWork.Vocabularies.GetAllPropertiesAsync();

        var propertiesByVocabId = allProperties
            .GroupBy(p => p.VocabularyId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var result = new List<VocabularyDto>();
        foreach (var vocab in vocabs)
        {
            var dto = _mapper.Map<VocabularyDto>(vocab);
            
            if (propertiesByVocabId.TryGetValue(vocab.Id, out var relatedProps))
            {
                dto.Properties = _mapper.Map<List<PropertyDto>>(relatedProps);
            }
            
            result.Add(dto);
        }

        return result;
    }
}