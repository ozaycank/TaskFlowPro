using FluentValidation;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlowPro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            // Validation Behavior pipeline will be added in Phase 5B
        });

        // Register FluentValidation Validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}