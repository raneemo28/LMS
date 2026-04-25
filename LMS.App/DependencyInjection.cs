using System.Reflection;
using AutoMapper;
using FluentValidation; // أضف هذا
using MediatR; // أضف هذا
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace LMS.App;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication2(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(assembly);
            
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); 
        });

        services.AddValidatorsFromAssembly(assembly);

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