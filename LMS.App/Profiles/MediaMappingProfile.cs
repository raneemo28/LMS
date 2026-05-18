using AutoMapper;
using System.Collections.Generic;
using LMS.App.DTOs.Media;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class MediaMappingProfile : Profile
{
    public MediaMappingProfile()
    {
        CreateMap<Media, MediaDto>()
            .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values));

        CreateMap<CreateMediaDto, Media>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt, opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
            .ForMember(d => d.Item, opt => opt.Ignore())
            .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values));

        CreateMap<UpdateMediaDto, Media>()
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt, opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
            .ForMember(d => d.Item, opt => opt.Ignore())
            .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values));

        CreateMap<Value, MetadataValueDto>()
            .ForCtorParam("propertyLabel", opt => opt.MapFrom(src => src.Property != null ? src.Property.Label : "Unknown Property"))
            .ForCtorParam("valueText", opt => opt.MapFrom(src => src.ValueText ?? src.ValueUri ?? string.Empty));

        CreateMap<Media, MediaWithMetadataDto>()
            .ForMember(d => d.Metadata, opt => opt.MapFrom(s => s.Values));
    }
}