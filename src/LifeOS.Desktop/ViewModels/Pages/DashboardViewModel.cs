using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Application.Activity;
using LifeOS.Application.Calendar;
using LifeOS.Application.Goals;
using LifeOS.Application.Habits;
using LifeOS.Application.Notes;
using LifeOS.Application.Study;
using LifeOS.Application.Tasks;
using LifeOS.Domain.Activity;
using LifeOS.Domain.Calendar;
using LifeOS.Domain.Notes;
using LifeOS.Domain.Tasks;
using LifeOS.Infrastructure.Settings;

namespace LifeOS.Desktop.ViewModels.Pages;

public sealed partial class DashboardViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    private readonly ITaskService _taskService;
    private readonly IHabitService _habitService;
    private readonly IStudyService _studyService;
    private readonly IGoalService _goalService;
    private readonly INoteService _noteService;
    private readonly ICalendarService _calendarService;
    private readonly IActivityLogService _activityService;

    [ObservableProperty] private string _greeting;
    [ObservableProperty] private string _todayLabel;
    [ObservableProperty] private string _newTaskTitle = string.Empty;

    // Quick stats
    [ObservableProperty] private int _tasksTodayCount;
    [ObservableProperty] private int _tasksCompletedToday;
    [ObservableProperty] private int _studyMinutesToday;
    [ObservableProperty] private int _habitsCompletedToday;
    [ObservableProperty] private int _habitsTotalToday;
    [ObservableProperty] private int _focusMinutesToday;
    [ObservableProperty] private int _bestStreak;

    public ObservableCollection<TaskItem> TodayTasks { get; } = new();
    public ObservableCollection<CalendarEvent> TodaySchedule { get; } = new();
    public ObservableCollection<HabitProgress> TodayHabits { get; } = new();
    public ObservableCollection<GoalProgress> TopGoals { get; } = new();
    public ObservableCollection<ActivityLogEntry> RecentActivity { get; } = new();

    [ObservableProperty] private Note? _latestNote;

    public DashboardViewModel(
        ITaskService taskService, IHabitService habitService, IStudyService studyService,
        IGoalService goalService, INoteService noteService, ICalendarService calendarService,
        IActivityLogService activityService , ISettingsService settingsService)
    {
        _taskService = taskService;
        _habitService = habitService;
        _studyService = studyService;
        _goalService = goalService;
        _noteService = noteService;
        _calendarService = calendarService;
        _activityService = activityService;
        _settingsService = settingsService;

        _greeting = BuildGreeting(_settingsService.Current.UserName);
        _todayLabel = DateTime.Now.ToString("dddd, MMMM d");


        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        // Tasks
       var tasks = await _taskService.GetTodayTasksAsync();
       TodayTasks.Clear();
       foreach (var t in tasks.Take(5)) TodayTasks.Add(t);
       TasksTodayCount = tasks.Count(t => !t.IsCompleted);
       TasksCompletedToday = tasks.Count(t => t.IsCompleted);
       
        // Habits
        var habits = await _habitService.GetTodayHabitsAsync();
        TodayHabits.Clear();
        foreach (var h in habits) TodayHabits.Add(h);
        HabitsTotalToday = habits.Count;
        HabitsCompletedToday = habits.Count(h => h.IsCompletedToday);
        BestStreak = habits.Count == 0 ? 0 : habits.Max(h => h.CurrentStreak);

        // Study / Pomodoro
        FocusMinutesToday = await _studyService.GetTodayFocusMinutesAsync();
        StudyMinutesToday = FocusMinutesToday;

        // Goals
        var goals = await _goalService.GetActiveGoalsAsync();
        TopGoals.Clear();
        foreach (var g in goals.Take(3)) TopGoals.Add(g);

        // Notes
        var notes = await _noteService.GetAllNotesAsync();
        LatestNote = notes.FirstOrDefault();

        // Schedule
        var events = await _calendarService.GetTodayEventsAsync();
        TodaySchedule.Clear();
        foreach (var e in events) TodaySchedule.Add(e);

        // Recent activity
        var activity = await _activityService.GetTodayAsync();
        RecentActivity.Clear();
        foreach (var a in activity.Take(10)) RecentActivity.Add(a);
    }

    [RelayCommand]
    private async Task AddTaskAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTaskTitle)) return;
        await _taskService.AddTaskAsync(NewTaskTitle, TaskPriority.Medium, DateTime.Today);
        NewTaskTitle = string.Empty;
        await LoadAsync();
    }

    [RelayCommand]
    private async Task CompleteTaskAsync(TaskItem task)
    {
        await _taskService.CompleteTaskAsync(task.Id);
        await LoadAsync();
    }

    private static string BuildGreeting(string name)
{
    var hour = DateTime.Now.Hour;
    var period = hour switch { < 12 => "Good morning", < 18 => "Good afternoon", _ => "Good evening" };
    return $"{period}, {name}";
}
}