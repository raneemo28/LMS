using AutoMapper;
using LMS.App.DTOs.Item;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ItemMappingProfile : Profile
{
    public ItemMappingProfile()
    {
        // Entity → DTO
        CreateMap<Item, ItemDto>()
            .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values));

        // DTO → Entity (create)
        CreateMap<CreateItemDto, Item>()
            .ForMember(d => d.Id,         opt => opt.Ignore())
            .ForMember(d => d.Type,       opt => opt.Ignore())
            .ForMember(d => d.OwnerId,    opt => opt.Ignore())
            .ForMember(d => d.CreatedAt,  opt => opt.Ignore())
            .ForMember(d => d.CreatedBy,  opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt, opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
            .ForMember(d => d.Template,   opt => opt.Ignore())
            .ForMember(d => d.Values,     opt => opt.MapFrom(s => s.Values));

        // DTO → Entity (update — maps onto existing tracked entity)
        CreateMap<UpdateItemDto, Item>()
            .ForMember(d => d.Type,       opt => opt.Ignore())
            .ForMember(d => d.OwnerId,    opt => opt.Ignore())
            .ForMember(d => d.CreatedAt,  opt => opt.Ignore())
            .ForMember(d => d.CreatedBy,  opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt, opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
            .ForMember(d => d.Template,   opt => opt.Ignore())
            .ForMember(d => d.Values,     opt => opt.MapFrom(s => s.Values));
    }
}