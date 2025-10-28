using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Common.Behaviors;
using System.Reflection;

namespace ProductManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR from Application assembly (where handlers are)
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Register FluentValidation validators from Application assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        return services;
    }
}