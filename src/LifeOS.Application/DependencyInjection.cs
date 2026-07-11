using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        // Use case handlers will be registered here as they are implemented,
        // e.g. services.AddScoped<ICreateTaskHandler, CreateTaskHandler>();

        return services;
    }
}
