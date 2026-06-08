using AutoMapper;
using LMS.App.DTOs.Media;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class MediaMappingProfile : Profile
{
    public MediaMappingProfile()
    {
        // Entity → DTO
        CreateMap<Media, MediaDto>()
            .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values));

        // Entity → metadata DTO (full map via mapper — handler no longer hand-builds this)
        CreateMap<Media, MediaWithMetadataDto>()
            .ForMember(d => d.Metadata, opt => opt.MapFrom(s => s.Values));

        // Value → metadata line
        CreateMap<Value, MetadataValueDto>()
            .ForCtorParam("propertyLabel",
                opt => opt.MapFrom(src => src.Property != null ? src.Property.Label : "Unknown Property"))
            .ForCtorParam("valueText",
                opt => opt.MapFrom(src => src.ValueText ?? src.ValueUri ?? string.Empty));

        // DTO → Entity (create)
        // OwnerId is NOT mapped from DTO — it is a server-side concern set by the handler.
        CreateMap<CreateMediaDto, Media>()
            .ForMember(d => d.Id,          opt => opt.Ignore())
            .ForMember(d => d.OwnerId,     opt => opt.Ignore())
            .ForMember(d => d.StoragePath, opt => opt.Ignore())
            .ForMember(d => d.MimeType,    opt => opt.Ignore())
            .ForMember(d => d.FileSize,    opt => opt.Ignore())
            .ForMember(d => d.CreatedAt,   opt => opt.Ignore())
            .ForMember(d => d.CreatedBy,   opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt,  opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy,  opt => opt.Ignore())
            .ForMember(d => d.Item,        opt => opt.Ignore())
            .ForMember(d => d.Type,        opt => opt.Ignore())
            .ForMember(d => d.Values,      opt => opt.MapFrom(s => s.Values));

        // DTO → Entity (update)
        CreateMap<UpdateMediaDto, Media>()
            .ForMember(d => d.OwnerId,     opt => opt.Ignore())
            .ForMember(d => d.StoragePath, opt => opt.Ignore())
            .ForMember(d => d.MimeType,    opt => opt.Ignore())
            .ForMember(d => d.FileSize,    opt => opt.Ignore())
            .ForMember(d => d.Type,        opt => opt.Ignore())
            .ForMember(d => d.CreatedAt,   opt => opt.Ignore())
            .ForMember(d => d.CreatedBy,   opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt,  opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy,  opt => opt.Ignore())
            .ForMember(d => d.Item,        opt => opt.Ignore())
            .ForMember(d => d.Values,      opt => opt.MapFrom(s => s.Values));
    }
}