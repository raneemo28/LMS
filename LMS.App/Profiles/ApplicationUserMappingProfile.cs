using AutoMapper;
using LMS.App.DTOs.Auth;
using LMS.Domain.Entities;
using LMS.App.Features.Register.Commands;

namespace LMS.App.Profiles;

public class ApplicationUserMappingProfile : Profile
{
    public ApplicationUserMappingProfile()
    {
        CreateMap<ApplicationUser, AuthResponse>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.Success, opt => opt.Ignore())
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore()); 

        CreateMap<RegisterCommand, ApplicationUser>()
    .ForMember(dest => dest.UserName,             opt => opt.MapFrom(src => src.Email))
    .ForMember(dest => dest.Id,                   opt => opt.Ignore())
    .ForMember(dest => dest.PhoneNumber,          opt => opt.MapFrom(src => src.PhoneNumber))
    .ForMember(dest => dest.PasswordHash,         opt => opt.Ignore())
    .ForMember(dest => dest.NormalizedEmail,      opt => opt.Ignore())
    .ForMember(dest => dest.NormalizedUserName,   opt => opt.Ignore())
    .ForMember(dest => dest.SecurityStamp,        opt => opt.Ignore())
    .ForMember(dest => dest.ConcurrencyStamp,     opt => opt.Ignore())
    .ForMember(dest => dest.EmailConfirmed,       opt => opt.Ignore())
    .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
    .ForMember(dest => dest.TwoFactorEnabled,     opt => opt.Ignore())
    .ForMember(dest => dest.LockoutEnd,           opt => opt.Ignore())
    .ForMember(dest => dest.LockoutEnabled,       opt => opt.Ignore())
    .ForMember(dest => dest.AccessFailedCount,    opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt,            opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
