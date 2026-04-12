using AutoMapper;
using LMS.App.DTOs.ApplicationUser;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class ApplicationUserMappingProfile : Profile
{
    public ApplicationUserMappingProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
            .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber));

        CreateMap<CreateApplicationUserDto, ApplicationUser>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Email))
            .ForMember(d => d.NormalizedUserName, opt => opt.Ignore())
            .ForMember(d => d.NormalizedEmail, opt => opt.Ignore())
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
            .ForMember(d => d.EmailConfirmed, opt => opt.Ignore())
            .ForMember(d => d.PasswordHash, opt => opt.Ignore())
            .ForMember(d => d.SecurityStamp, opt => opt.Ignore())
            .ForMember(d => d.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber))
            .ForMember(d => d.PhoneNumberConfirmed, opt => opt.Ignore())
            .ForMember(d => d.TwoFactorEnabled, opt => opt.Ignore())
            .ForMember(d => d.LockoutEnd, opt => opt.Ignore())
            .ForMember(d => d.LockoutEnabled, opt => opt.Ignore())
            .ForMember(d => d.AccessFailedCount, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore());

        CreateMap<UpdateApplicationUserDto, ApplicationUser>()
            .ForMember(d => d.UserName, opt => opt.Ignore())
            .ForMember(d => d.NormalizedUserName, opt => opt.Ignore())
            .ForMember(d => d.NormalizedEmail, opt => opt.Ignore())
            .ForMember(d => d.Email, opt => opt.Ignore())
            .ForMember(d => d.EmailConfirmed, opt => opt.Ignore())
            .ForMember(d => d.PasswordHash, opt => opt.Ignore())
            .ForMember(d => d.SecurityStamp, opt => opt.Ignore())
            .ForMember(d => d.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(d => d.PhoneNumberConfirmed, opt => opt.Ignore())
            .ForMember(d => d.TwoFactorEnabled, opt => opt.Ignore())
            .ForMember(d => d.LockoutEnd, opt => opt.Ignore())
            .ForMember(d => d.LockoutEnabled, opt => opt.Ignore())
            .ForMember(d => d.AccessFailedCount, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore());
    }
}
