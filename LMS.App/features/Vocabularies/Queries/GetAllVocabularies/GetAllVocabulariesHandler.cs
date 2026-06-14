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
        // 1. Fetch all vocabularies (Query #1)
        var vocabs = await _unitOfWork.Vocabularies.GetAllAsync();
        
        // 2. Fetch ALL properties in a single query (Query #2)
        // We do this instead of looping to completely avoid the N+1 problem.
        var allProperties = await _unitOfWork.Vocabularies.GetAllPropertiesAsync();
        
        // 3. Group properties by VocabularyId in memory for O(1) instant lookup
        // (تجميع الخصائص في الذاكرة للبحث الفوري)
        var propertiesByVocabId = allProperties
            .GroupBy(p => p.VocabularyId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // 4. Map vocabularies and attach their properties
        var result = new List<VocabularyDto>();
        foreach (var vocab in vocabs)
        {
            // AutoMapper maps the base Vocabulary entity to the DTO
            var dto = _mapper.Map<VocabularyDto>(vocab);
            
            // Try to find the properties for this specific vocabulary in our Dictionary
            if (propertiesByVocabId.TryGetValue(vocab.Id, out var relatedProps))
            {
                // AutoMapper maps the list of Property entities to a list of PropertyDtos
                dto.Properties = _mapper.Map<List<PropertyDto>>(relatedProps);
            }
            
            result.Add(dto);
        }

        return result;
    }
}