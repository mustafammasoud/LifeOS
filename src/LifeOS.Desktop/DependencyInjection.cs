using LifeOS.Application.Notes;
using LifeOS.Desktop.Services;
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
        services.AddTransient<HabitsViewModel>();   
        services.AddTransient<StudyViewModel>();   
        services.AddTransient<GoalsViewModel>();
        services.AddTransient<NotesViewModel>();
        services.AddTransient<CalendarViewModel>();
        services.AddTransient<TasksViewModel>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddTransient<StatisticsViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddScoped<INoteService, NoteService>();
        
        

        return services;
    }
}
