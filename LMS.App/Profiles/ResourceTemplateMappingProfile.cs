using AutoMapper;
using LMS.App.DTOs.ResourceTemplate;
using LMS.App.DTOs.ResourceProperty;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ResourceTemplateMappingProfile : Profile
{
    public ResourceTemplateMappingProfile()
    {
        // Entity → DTO
        CreateMap<ResourceTemplate, ResourceTemplateDto>()
            .ForMember(d => d.Properties, opt => opt.MapFrom(s => s.TemplateProperties));

        CreateMap<TemplateProperty, ResourcePropertyDto>()
            .ForMember(d => d.LocalName,
                opt => opt.MapFrom(src => src.Property != null ? src.Property.LocalName : string.Empty))
            .ForMember(d => d.Label,
                opt => opt.MapFrom(src => src.Property != null ? src.Property.Label : string.Empty))
            .ForMember(d => d.TermUri,
                opt => opt.MapFrom(src => src.Property != null ? src.Property.TermUri : string.Empty));

        // DTO → Entity (create template)
        CreateMap<CreateResourceTemplateDto, ResourceTemplate>()
            .ForMember(d => d.Id,                 opt => opt.Ignore())
            .ForMember(d => d.TemplateProperties, opt => opt.Ignore());

        // DTO → Entity (update template — maps onto existing tracked entity)
        CreateMap<UpdateResourceTemplateDto, ResourceTemplate>()
            .ForMember(d => d.Id,                 opt => opt.Ignore())
            .ForMember(d => d.TemplateProperties, opt => opt.Ignore());

        // DTO → Entity (template property link)
        CreateMap<ResourcePropertyDto, TemplateProperty>()
            .ForMember(d => d.TemplateId,     opt => opt.Ignore())
            .ForMember(d => d.Template,       opt => opt.Ignore())
            .ForMember(d => d.Property,       opt => opt.Ignore());
    }
}