namespace LifeOS.Domain.Settings;

public enum AppTheme { Dark, Light }

public class AppSettings
{
    public string UserName { get; set; } = "there";
    public AppTheme Theme { get; set; } = AppTheme.Dark;
    public bool NotificationsEnabled { get; set; } = true;
    public int ReminderMinutesBeforeEvent { get; set; } = 10;
}
