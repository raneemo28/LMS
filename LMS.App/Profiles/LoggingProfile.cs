using AutoMapper;
using LMS.App.DTOs.Logging;
using LMS.Domain.Entities;

namespace LMS.App.Profiles;

public class LoggingProfile : Profile
{
    public LoggingProfile()
    {
        CreateMap<LogDTO, LogEntry>()
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.Id, opt => opt.Ignore());
        
    }
}