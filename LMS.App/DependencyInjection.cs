using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace LMS.App;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication2(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        var assembly = Assembly.GetExecutingAssembly();
        services.AddSingleton<IMapper>(_ =>
        {
            var config = new MapperConfiguration(
                cfg => cfg.AddMaps(assembly),
                NullLoggerFactory.Instance);
            return config.CreateMapper();
        });

        return services;
    }
}