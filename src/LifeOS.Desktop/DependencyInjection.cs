using LifeOS.Desktop.ViewModels;
using LifeOS.Desktop.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Desktop;

public static class DependencyInjection
{
    public static IServiceCollection AddDesktopShell(this IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<DashboardViewModel>();

        return services;
    }
}
