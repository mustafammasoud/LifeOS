using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using LifeOS.Desktop.Navigation;
using LifeOS.Desktop.Services;
using LifeOS.Desktop.ViewModels.Pages;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Desktop.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly IServiceProvider _services;

    private readonly INavigationService _navigation;

    public ObservableCollection<NavigationItemViewModel> NavigationItems { get; }

    [ObservableProperty]
    private NavigationItemViewModel _selectedNavigationItem;

    [ObservableProperty]
    private ObservableObject _currentPage;

    public MainWindowViewModel(
    IServiceProvider services,
    INavigationService navigation)
{
    _services = services;
    _navigation = navigation;

        NavigationItems = new ObservableCollection<NavigationItemViewModel>
        {
            new(PageKey.Dashboard, "Dashboard", MaterialIconKind.ViewDashboardOutline),
            new(PageKey.Tasks, "Tasks", MaterialIconKind.CheckboxMarkedOutline),
            new(PageKey.Study, "Study", MaterialIconKind.BookOpenPageVariantOutline),
            new(PageKey.Habits, "Habits", MaterialIconKind.Repeat),
            new(PageKey.Goals, "Goals", MaterialIconKind.FlagOutline),
            new(PageKey.Calendar, "Events", MaterialIconKind.CalendarMonthOutline),
            new(PageKey.Notes, "Notes", MaterialIconKind.NoteTextOutline),
            new(PageKey.Statistics, "Statistics", MaterialIconKind.ChartBoxOutline),
            new(PageKey.Settings, "Settings", MaterialIconKind.CogOutline),
        };

        _selectedNavigationItem = NavigationItems[0];
        _currentPage = ResolvePage(PageKey.Dashboard);
        _navigation.NavigateRequested += page =>
      {
          var item = NavigationItems.First(x => x.Key == page);
      
          SelectedNavigationItem = item;
      };
    }

    partial void OnSelectedNavigationItemChanged(NavigationItemViewModel value)
    {
        CurrentPage = ResolvePage(value.Key);
    }

    private ObservableObject ResolvePage(PageKey key) => key switch
{
    PageKey.Dashboard => (ObservableObject)_services.GetRequiredService(typeof(DashboardViewModel)),
    PageKey.Tasks => (ObservableObject)_services.GetRequiredService(typeof(TasksViewModel)),
    PageKey.Habits => (ObservableObject)_services.GetRequiredService(typeof(HabitsViewModel)),
    PageKey.Study => (ObservableObject)_services.GetRequiredService(typeof(StudyViewModel)),
    PageKey.Goals => (ObservableObject)_services.GetRequiredService(typeof(GoalsViewModel)),
    PageKey.Notes => (ObservableObject)_services.GetRequiredService(typeof(NotesViewModel)),
    PageKey.Calendar => (ObservableObject)_services.GetRequiredService(typeof(CalendarViewModel)),
    PageKey.Statistics => (ObservableObject)_services.GetRequiredService(typeof(StatisticsViewModel)),
    PageKey.Settings => (ObservableObject)_services.GetRequiredService(typeof(SettingsViewModel)),
    _ => new ComingSoonViewModel(key.ToString())
};
}
