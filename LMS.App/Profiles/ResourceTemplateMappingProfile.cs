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
            .ForMember(d => d.Properties, opt => opt.Ignore()); 
        CreateMap<ResourcePropertyDto, TemplateProperty>()
            .ForMember(d => d.TemplateId, opt => opt.Ignore())
            .ForMember(d => d.Template, opt => opt.Ignore())
            .ForMember(d => d.Property, opt => opt.Ignore());
    }
}
