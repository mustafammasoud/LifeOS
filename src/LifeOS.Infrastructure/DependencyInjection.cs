using LifeOS.Infrastructure.Notifications;
using LifeOS.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<INotificationService, CrossPlatformNotificationService>();
        services.AddSingleton<ISettingsService, SettingsService>();

        return services;
    }
}