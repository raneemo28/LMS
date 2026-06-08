using AutoMapper;
using LMS.App.DTOs.ItemSet;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ItemSetMappingProfile : Profile
{
    public ItemSetMappingProfile()
    {
        // Entity → DTO
        CreateMap<ItemSet, ItemSetDto>()
            .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values));

        // Composite → DTO
        CreateMap<ItemSetWithMembers, ItemSetMembersDto>()
            .ForMember(d => d.SetInfo, opt => opt.MapFrom(s => s.SetInfo))
            .ForMember(d => d.Members, opt => opt.MapFrom(s => s.Members));

        // DTO → Entity (create)
        CreateMap<CreateItemSetDto, ItemSet>()
            .ForMember(d => d.Id,         opt => opt.Ignore())
            .ForMember(d => d.Type,       opt => opt.Ignore())
            .ForMember(d => d.OwnerId,    opt => opt.Ignore())
            .ForMember(d => d.CreatedAt,  opt => opt.Ignore())
            .ForMember(d => d.CreatedBy,  opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt, opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
            .ForMember(d => d.Values,     opt => opt.MapFrom(s => s.Values));

        // DTO → Entity (update — maps onto existing tracked entity)
        CreateMap<UpdateItemSetDto, ItemSet>()
            .ForMember(d => d.Type,       opt => opt.Ignore())
            .ForMember(d => d.OwnerId,    opt => opt.Ignore())
            .ForMember(d => d.CreatedAt,  opt => opt.Ignore())
            .ForMember(d => d.CreatedBy,  opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt, opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
            .ForMember(d => d.Values,     opt => opt.MapFrom(s => s.Values));
    }
}