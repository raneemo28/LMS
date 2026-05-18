using AutoMapper;
using LMS.App.DTOs.Vocabulary;
using LMS.App.DTOs.Property;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class VocabularyMappingProfile : Profile
{
    public VocabularyMappingProfile()
    {
        CreateMap<Vocabulary, VocabularyDto>();
        CreateMap<Property, PropertyDto>();

        CreateMap<CreateVocabularyDto, Vocabulary>()
            .ForMember(d => d.Id, opt => opt.Ignore());

        CreateMap<UpdateVocabularyDto, Vocabulary>()
            .ForMember(d => d.Id, opt => opt.Ignore());
    }
}
