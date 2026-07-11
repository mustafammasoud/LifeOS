using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Future: notification service, file system service, Google Calendar client,
        // AI assistant client. Kept empty intentionally (YAGNI).

        return services;
    }
}
