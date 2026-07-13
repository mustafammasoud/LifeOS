using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Domain.Settings;
using LifeOS.Infrastructure.Settings;

namespace LifeOS.Desktop.ViewModels.Pages;

public sealed partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    [ObservableProperty] private string _userName;
    [ObservableProperty] private AppTheme _theme;
    [ObservableProperty] private bool _notificationsEnabled;
    [ObservableProperty] private int _reminderMinutes;

    public string DatabasePath => _settingsService.DatabasePath;
    public Array ThemeOptions { get; } = Enum.GetValues(typeof(AppTheme));

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;

        var current = settingsService.Current;
        _userName = current.UserName;
        _theme = current.Theme;
        _notificationsEnabled = current.NotificationsEnabled;
        _reminderMinutes = current.ReminderMinutesBeforeEvent;
    }

    [RelayCommand]
    private void Save()
    {
        _settingsService.Save(new AppSettings
        {
            UserName = UserName,
            Theme = Theme,
            NotificationsEnabled = NotificationsEnabled,
            ReminderMinutesBeforeEvent = ReminderMinutes
        });

        if (App.Current is not null)
        {
            App.Current.RequestedThemeVariant = Theme == AppTheme.Dark
                ? Avalonia.Styling.ThemeVariant.Dark
                : Avalonia.Styling.ThemeVariant.Light;
        }
    }
}
