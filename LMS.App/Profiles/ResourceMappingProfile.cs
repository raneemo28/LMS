using AutoMapper;
using LMS.App.DTOs.Resource;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ResourceMappingProfile : Profile
{
    public ResourceMappingProfile()
    {
        CreateMap<Resource, ResourceDto>();
    }
}