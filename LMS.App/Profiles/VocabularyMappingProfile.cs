using AutoMapper;
using LMS.App.DTOs.Vocabulary;
using LMS.App.DTOs.Property;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class VocabularyMappingProfile : Profile
{
    public VocabularyMappingProfile()
    {
        // Entity → DTO
        CreateMap<Vocabulary, VocabularyDto>();
        CreateMap<Property, PropertyDto>();

        // DTO → Entity (create)
        CreateMap<CreateVocabularyDto, Vocabulary>()
            .ForMember(d => d.Id, opt => opt.Ignore());

        // DTO → Entity (update)
        CreateMap<UpdateVocabularyDto, Vocabulary>()
            .ForMember(d => d.Id, opt => opt.Ignore());

        // DTO → Entity (property create)
        CreateMap<CreatePropertyDto, Property>()
            .ForMember(d => d.Id,         opt => opt.Ignore())
            .ForMember(d => d.VocabularyId, opt => opt.Ignore())
            .ForMember(d => d.Vocabulary, opt => opt.Ignore());

        // DTO → Entity (property update)
        CreateMap<UpdatePropertyDto, Property>()
            .ForMember(d => d.Id,           opt => opt.Ignore())
            .ForMember(d => d.VocabularyId, opt => opt.Ignore())
            .ForMember(d => d.Vocabulary,   opt => opt.Ignore());
    }
}