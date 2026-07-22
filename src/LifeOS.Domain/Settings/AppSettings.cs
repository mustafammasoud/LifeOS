namespace LifeOS.Domain.Settings;

public enum AppTheme { Dark, Light }

public class AppSettings
{
    public string UserName { get; set; } = "there";
    public AppTheme Theme { get; set; } = AppTheme.Dark;
    public bool NotificationsEnabled { get; set; } = true;
    public int ReminderMinutesBeforeEvent { get; set; } = 10;

    public int StudyRemainingSeconds { get; set; } = 25 * 60;  
    public int StudyDurationMinutes { get; set; } = 25;
    public bool StudyIsRunning { get; set; } = false;
    public DateTime? StudyTargetEndTimeUtc { get; set; }    
    public Guid? StudySelectedSubjectId { get; set; }
}