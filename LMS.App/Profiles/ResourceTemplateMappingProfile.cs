using AutoMapper;
using LMS.App.DTOs.ResourceTemplate;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ResourceTemplateMappingProfile : Profile
{
    public ResourceTemplateMappingProfile()
    {
        CreateMap<TemplateProperty, ResourceTemplatePropertyDto>();

        CreateMap<ResourceTemplate, ResourceTemplateDto>()
            .ForMember(d => d.Properties, opt => opt.MapFrom(s => s.Properties));

        CreateMap<CreateResourceTemplatePropertyDto, TemplateProperty>()
            .ForMember(d => d.TemplateId, opt => opt.Ignore())
            .ForMember(d => d.Template, opt => opt.Ignore())
            .ForMember(d => d.Property, opt => opt.Ignore());

        CreateMap<UpdateResourceTemplatePropertyDto, TemplateProperty>()
            .ForMember(d => d.TemplateId, opt => opt.Ignore())
            .ForMember(d => d.Template, opt => opt.Ignore())
            .ForMember(d => d.Property, opt => opt.Ignore());

        CreateMap<CreateResourceTemplateDto, ResourceTemplate>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Properties, opt => opt.MapFrom(s => s.Properties))
            .ForMember(d => d.Items, opt => opt.Ignore());

        CreateMap<UpdateResourceTemplateDto, ResourceTemplate>()
            .ForMember(d => d.Properties, opt => opt.MapFrom(s => s.Properties))
            .ForMember(d => d.Items, opt => opt.Ignore());
    }
}
