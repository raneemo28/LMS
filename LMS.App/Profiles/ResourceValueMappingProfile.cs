using AutoMapper;
using LMS.App.DTOs.Value;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ResourceValueMappingProfile : Profile
{
    public ResourceValueMappingProfile()
    {
        CreateMap<Value, ResourceValueDto>();

        CreateMap<CreateResourceValueDto, Value>()
            .ForMember(d => d.Id,              opt => opt.Ignore())
            .ForMember(d => d.ResourceId,      opt => opt.Ignore())
            .ForMember(d => d.ValueUri,        opt => opt.Ignore()) 
            .ForMember(d => d.Resource,        opt => opt.Ignore())
            .ForMember(d => d.Property,        opt => opt.Ignore())
            .ForMember(d => d.ValueResource,   opt => opt.Ignore());

        CreateMap<UpdateResourceValueDto, Value>()
            .ForMember(d => d.Id,              opt => opt.Ignore())
            .ForMember(d => d.ResourceId,      opt => opt.Ignore())
            .ForMember(d => d.PropertyId,      opt => opt.Ignore())
            .ForMember(d => d.ValueUri,        opt => opt.Ignore())
            .ForMember(d => d.Resource,        opt => opt.Ignore())
            .ForMember(d => d.Property,        opt => opt.Ignore())
            .ForMember(d => d.ValueResource,   opt => opt.Ignore());
    }
}