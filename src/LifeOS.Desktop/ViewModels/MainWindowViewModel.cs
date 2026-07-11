using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LifeOS.Desktop.Navigation;
using LifeOS.Desktop.ViewModels.Pages;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Desktop.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly IServiceProvider _services;

    public ObservableCollection<NavigationItemViewModel> NavigationItems { get; }

    [ObservableProperty]
    private NavigationItemViewModel _selectedNavigationItem;

    [ObservableProperty]
    private ObservableObject _currentPage;

    public MainWindowViewModel(IServiceProvider services)
    {
        _services = services;

        NavigationItems = new ObservableCollection<NavigationItemViewModel>
        {
            new(PageKey.Dashboard, "Dashboard", MaterialIconKind.ViewDashboardOutline),
            new(PageKey.Tasks, "Tasks", MaterialIconKind.CheckboxMarkedOutline),
            new(PageKey.Study, "Study", MaterialIconKind.BookOpenPageVariantOutline),
            new(PageKey.Habits, "Habits", MaterialIconKind.Repeat),
            new(PageKey.Goals, "Goals", MaterialIconKind.FlagOutline),
            new(PageKey.Calendar, "Calendar", MaterialIconKind.CalendarMonthOutline),
            new(PageKey.Pomodoro, "Pomodoro", MaterialIconKind.TimerOutline),
            new(PageKey.Notes, "Notes", MaterialIconKind.NoteTextOutline),
            new(PageKey.Statistics, "Statistics", MaterialIconKind.ChartBoxOutline),
            new(PageKey.Settings, "Settings", MaterialIconKind.CogOutline),
        };

        _selectedNavigationItem = NavigationItems[0];
        _currentPage = ResolvePage(PageKey.Dashboard);
    }

    partial void OnSelectedNavigationItemChanged(NavigationItemViewModel value)
    {
        CurrentPage = ResolvePage(value.Key);
    }

    private ObservableObject ResolvePage(PageKey key) => key switch
    {
        PageKey.Dashboard => (ObservableObject)_services.GetRequiredService(typeof(DashboardViewModel)),
        _ => new ComingSoonViewModel(key.ToString())
    };
}
