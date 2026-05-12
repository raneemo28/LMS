using AutoMapper;
using LMS.App.DTOs.ResourceTemplate;
using LMS.App.DTOs.ResourceProperty;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ResourceTemplateMappingProfile : Profile
{
    public ResourceTemplateMappingProfile()
    {
        CreateMap<TemplateProperty, ResourcePropertyDto>();

        CreateMap<ResourceTemplate, ResourceTemplateDto>()
            .ForMember(d => d.Properties, opt => opt.MapFrom(s => s.TemplateProperties)); 
        CreateMap<ResourcePropertyDto, TemplateProperty>()
            .ForMember(dest => dest.TemplateId,     opt => opt.Ignore())   // set by handler
            .ForMember(dest => dest.PropertyId,     opt => opt.MapFrom(src => src.PropertyId))
            .ForMember(dest => dest.IsRequired,     opt => opt.MapFrom(src => src.IsRequired))
            .ForMember(dest => dest.DisplayOrder,   opt => opt.MapFrom(src => src.DisplayOrder))
            .ForMember(dest => dest.AlternateLabel, opt => opt.MapFrom(src => src.AlternateLabel))
            .ForMember(dest => dest.Template,       opt => opt.Ignore())
            .ForMember(dest => dest.Property,       opt => opt.Ignore());
    }
}
