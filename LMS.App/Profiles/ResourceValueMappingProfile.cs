using AutoMapper;
using LMS.App.DTOs.Value;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ResourceValueMappingProfile : Profile
{
    public ResourceValueMappingProfile()
    {
        // Entity → DTO
        CreateMap<Value, ResourceValueDto>();

        // DTO → Entity (create)
        CreateMap<CreateResourceValueDto, Value>()
            .ForMember(d => d.Id,              opt => opt.Ignore())
            .ForMember(d => d.ResourceId,      opt => opt.Ignore())
            .ForMember(d => d.ValueUri,        opt => opt.Ignore()) // system-derived, not user-supplied
            .ForMember(d => d.Resource,        opt => opt.Ignore())
            .ForMember(d => d.Property,        opt => opt.Ignore())
            .ForMember(d => d.ValueResource,   opt => opt.Ignore());

        // DTO → Entity (update — partial, ValueUri is system-derived so also ignored)
        CreateMap<UpdateResourceValueDto, Value>()
            .ForMember(d => d.Id,              opt => opt.Ignore())
            .ForMember(d => d.ResourceId,      opt => opt.Ignore())
            .ForMember(d => d.PropertyId,      opt => opt.Ignore())
            .ForMember(d => d.ValueUri,        opt => opt.Ignore())
            .ForMember(d => d.Resource,        opt => opt.Ignore())
            .ForMember(d => d.Property,        opt => opt.Ignore())
            .ForMember(d => d.ValueResource,   opt => opt.Ignore());

        // =====================================================================
        // THE FIX: Map ResourceValueDto to Value
        // This is required because UpdateItemDto contains List<ResourceValueDto>
        // and AutoMapper needs to know how to map it to List<Value>.
        // =====================================================================
        CreateMap<ResourceValueDto, Value>()
            .ForMember(d => d.ResourceId,    opt => opt.Ignore()) // Will be set by AfterMap in ItemMappingProfile
            .ForMember(d => d.ValueUri,      opt => opt.Ignore()) // System-derived
            .ForMember(d => d.Resource,      opt => opt.Ignore()) // Navigation property
            .ForMember(d => d.Property,      opt => opt.Ignore()) // Navigation property
            .ForMember(d => d.ValueResource, opt => opt.Ignore()); // Navigation property
    }
}