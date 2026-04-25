using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using LMS.App.Behaviors;

namespace LMS.App;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication2(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // 1. Register MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        // 2. Register FluentValidation - auto-discovers all IValidator<T> implementations
        services.AddValidatorsFromAssembly(assembly);

        // 3. Register Validation Pipeline Behavior - runs validators automatically before handlers
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // 4. Register AutoMapper
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