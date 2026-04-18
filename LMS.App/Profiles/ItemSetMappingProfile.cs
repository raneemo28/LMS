using AutoMapper;
using LMS.App.DTOs.ItemSet;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ItemSetMappingProfile : Profile
{
    public ItemSetMappingProfile()
    {
        CreateMap<ItemSet, ItemSetMembersDto>();
        CreateMap<ItemSet, ItemSetDto>()
            .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values));

        CreateMap<CreateItemSetDto, ItemSet>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt, opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
            .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values));

        CreateMap<UpdateItemSetDto, ItemSet>()
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.ModifiedAt, opt => opt.Ignore())
            .ForMember(d => d.ModifiedBy, opt => opt.Ignore())
            .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values));
    }
}
