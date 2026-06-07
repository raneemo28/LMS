using AutoMapper;
using LMS.App.Profiles;
using Microsoft.Extensions.Logging;

namespace LMS.Tests.UnitTests.AutoMapper;

public abstract class AutoMapperTestsBase
{
    protected readonly IMapper Mapper;

    protected AutoMapperTestsBase()
    {
        var loggerFactory = LoggerFactory.Create(_ => { });

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApplicationUserMappingProfile>();
            cfg.AddProfile<ItemMappingProfile>();
            cfg.AddProfile<ItemSetMappingProfile>();
            cfg.AddProfile<LoggingProfile>();
            cfg.AddProfile<MediaMappingProfile>();
            cfg.AddProfile<ResourceTemplateMappingProfile>();
            cfg.AddProfile<ResourceValueMappingProfile>();
            cfg.AddProfile<VocabularyMappingProfile>();
        }, loggerFactory);

        Mapper = config.CreateMapper();
    }
}