using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Application.Goals;
using LifeOS.Application.Habits;
using LifeOS.Application.Statistics;
using LifeOS.Application.Study;
using LifeOS.Application.Tasks;

namespace LifeOS.Desktop.ViewModels.Pages;

public class SubjectStat { public required string Name { get; init; } public int Minutes { get; init; } public int PercentOfMax { get; init; } }
public class DayStat
{
    public required string DayLabel { get; init; }

    public int Count { get; init; }

    public int Height { get; init; }

    public string Color { get; init; } = "#4ADE80";
}

public class HabitStat { public required string Name { get; init; } public int Streak { get; init; } }

public sealed partial class StatisticsViewModel : ObservableObject
{
    private readonly IDailyStatisticsService _statisticsService;
    private readonly IHabitService _habitService;
    private readonly IStudyService _studyService;
    private readonly IGoalService _goalService;

    public double WeeklyTaskCompletionPercent { get; private set; }
    public double HabitsCompletionPercent { get; private set; }
    public double GoalsCompletionPercent { get; private set; }
    public double StudyFocusPercent { get; private set; }

    public int TotalTasksCompleted { get; private set; }
    public int TotalStudyMinutes { get; private set; }
    public int LongestStreak { get; private set; }
    public int GoalsCompleted { get; private set; }
    public int GoalsActive { get; private set; }

    public ObservableCollection<DayStat> WeeklyTasks { get; } = new();
    public ObservableCollection<SubjectStat> SubjectBreakdown { get; } = new();
    public ObservableCollection<HabitStat> HabitStreaks { get; } = new();

    public StatisticsViewModel(
    IDailyStatisticsService statisticsService,
    IHabitService habitService,
    IStudyService studyService,
    IGoalService goalService)
    {
    _statisticsService = statisticsService;
        _habitService = habitService;
        _studyService = studyService;
        _goalService = goalService;
        _ = LoadAsync();
    }

    [RelayCommand]
    public async Task Reload()
    {
        await LoadAsync();
    }

    public async Task LoadAsync()
{
    // ==========================
    // Tasks Statistics
    // ==========================

    var today = DateOnly.FromDateTime(DateTime.Today);

    // هنخليها السبت مؤقتًا
    var startOfWeek = today.AddDays(-(int)today.DayOfWeek);

    var week = await _statisticsService.GetWeekAsync(startOfWeek);

    var created = week.Sum(x => x.TasksCreated);
    var completed = week.Sum(x => x.TasksCompleted);

    TotalTasksCompleted = completed;

    WeeklyTaskCompletionPercent =
        created == 0
            ? 0
            : completed * 100.0 / created;

    WeeklyTasks.Clear();

    var maxCompleted = Math.Max(1, week.Max(x => x.TasksCompleted));

    foreach (var day in week.OrderBy(x => x.Date))
    {  var height = Math.Max(
          12,
          (int)(day.TasksCompleted * 120.0 / maxCompleted));
      
      WeeklyTasks.Add(new DayStat
      {
          DayLabel = day.Date.ToString("ddd"),
          Count = day.TasksCompleted,
          Height = height,
          Color = GetBarColor(day.TasksCompleted)
      });}

    // ==========================
    // Study
    // ==========================

    var summaries = await _studyService.GetSubjectSummariesAsync();

    TotalStudyMinutes = summaries.Sum(s => s.TotalMinutes);

    var maxMinutes = Math.Max(
        1,
        summaries.Select(s => s.TotalMinutes)
                 .DefaultIfEmpty(0)
                 .Max());

    SubjectBreakdown.Clear();

    foreach (var s in summaries.OrderByDescending(s => s.TotalMinutes))
    {
        SubjectBreakdown.Add(new SubjectStat
        {
            Name = s.SubjectName,
            Minutes = s.TotalMinutes,
            PercentOfMax = (int)(s.TotalMinutes * 100.0 / maxMinutes)
        });
    }

    var todayFocus = await _studyService.GetTodayFocusMinutesAsync();

    StudyFocusPercent = Math.Min(100, todayFocus / 240.0 * 100);

    // ==========================
    // Habits
    // ==========================

    var habits = await _habitService.GetTodayHabitsAsync();

    LongestStreak = habits.Count == 0
        ? 0
        : habits.Max(h => h.CurrentStreak);

    HabitsCompletionPercent = habits.Count == 0
        ? 0
        : habits.Count(h => h.IsCompletedToday) * 100.0 / habits.Count;

    // ==========================
    // Goals
    // ==========================

    var (completedGoals, activeGoals) =
        await _goalService.GetGoalsSummaryAsync();

    GoalsCompleted = completedGoals;
    GoalsActive = activeGoals;

    GoalsCompletionPercent =
        (completedGoals + activeGoals) == 0
            ? 0
            : completedGoals * 100.0 / (completedGoals + activeGoals);

    OnPropertyChanged(nameof(WeeklyTaskCompletionPercent));
    OnPropertyChanged(nameof(HabitsCompletionPercent));
    OnPropertyChanged(nameof(GoalsCompletionPercent));
    OnPropertyChanged(nameof(StudyFocusPercent));
}

private static string GetBarColor(int count)
{
    return count switch
    {
        0 => "#404040",
        <= 2 => "#4ADE80",
        <= 5 => "#60A5FA",
        <= 8 => "#A855F7",
        _ => "#EF4444"
    };
}

}