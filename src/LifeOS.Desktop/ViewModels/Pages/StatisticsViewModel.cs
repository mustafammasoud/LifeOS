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

public class WeekTrendStat
{
    public required string WeekLabel { get; init; }
    public double Percent { get; init; }
    public int Height { get; init; }
    public bool IsSelected { get; init; }
}

public sealed partial class StatisticsViewModel : ObservableObject
{
    private readonly IDailyStatisticsService _statisticsService;
    private readonly IHabitService _habitService;
    private readonly IStudyService _studyService;
    private readonly IGoalService _goalService;

    private int _weekOffset; // 0 = الأسبوع الحالي، -1 = اللي فات، وهكذا

    [ObservableProperty] private double _weeklyTaskCompletionPercent;
    [ObservableProperty] private double _habitsCompletionPercent;
    [ObservableProperty] private double _goalsCompletionPercent;
    [ObservableProperty] private double _studyFocusPercent;

    [ObservableProperty] private int _totalTasksCompleted;
    [ObservableProperty] private int _totalStudyMinutes;
    [ObservableProperty] private int _longestStreak;
    [ObservableProperty] private int _goalsCompleted;
    [ObservableProperty] private int _goalsActive;

    [ObservableProperty] private string _weekRangeLabel = string.Empty;
    [ObservableProperty] private bool _canGoToNextWeek;

    public ObservableCollection<DayStat> WeeklyTasks { get; } = new();
    public ObservableCollection<SubjectStat> SubjectBreakdown { get; } = new();
    public ObservableCollection<HabitStat> HabitStreaks { get; } = new();
    public ObservableCollection<WeekTrendStat> WeeklyTrend { get; } = new();

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

    [RelayCommand]
    private async Task PreviousWeek()
    {
        _weekOffset--;
        await LoadAsync();
    }

    [RelayCommand]
    private async Task NextWeek()
    {
        if (_weekOffset >= 0) return;
        _weekOffset++;
        await LoadAsync();
    }

    private static DateOnly GetSaturdayWeekStart(DateOnly date)
    {
        var daysSinceSaturday = ((int)date.DayOfWeek + 1) % 7;
        return date.AddDays(-daysSinceSaturday);
    }

    public async Task LoadAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var currentWeekStart = GetSaturdayWeekStart(today);
        var startOfWeek = currentWeekStart.AddDays(_weekOffset * 7);

        CanGoToNextWeek = _weekOffset < 0;
        WeekRangeLabel = _weekOffset == 0
            ? "This Week"
            : $"{startOfWeek:MMM d} – {startOfWeek.AddDays(6):MMM d}";

        // ==========================
        // Tasks Statistics (للأسبوع المختار)
        // ==========================

        var week = await _statisticsService.GetWeekAsync(startOfWeek);

        var created = week.Sum(x => x.TasksCreated);
        var completed = week.Sum(x => x.TasksCompleted);

        TotalTasksCompleted = completed;

        WeeklyTaskCompletionPercent =
            created == 0
                ? 0
                : Math.Min(100, completed * 100.0 / created);

        WeeklyTasks.Clear();

        var maxCompleted = Math.Max(1, week.Max(x => x.TasksCompleted));

        foreach (var day in week.OrderBy(x => x.Date))
        {
            var height = Math.Max(12, (int)(day.TasksCompleted * 120.0 / maxCompleted));

            WeeklyTasks.Add(new DayStat
            {
                DayLabel = day.Date.ToString("ddd"),
                Count = day.TasksCompleted,
                Height = height,
                Color = GetBarColor(day.TasksCompleted)
            });
        }

        // ==========================
        // Study (للأسبوع المختار)
        // ==========================

        var summaries = await _studyService.GetSubjectSummariesForWeekAsync(startOfWeek);

        TotalStudyMinutes = summaries.Sum(s => s.TotalMinutes);

        var maxMinutes = Math.Max(1, summaries.Select(s => s.TotalMinutes).DefaultIfEmpty(0).Max());

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
        // Habits (دايمًا النهارده، مش متأثرة بالتنقل)
        // ==========================

        var habits = await _habitService.GetTodayHabitsAsync();

        LongestStreak = habits.Count == 0 ? 0 : habits.Max(h => h.CurrentStreak);

        HabitsCompletionPercent = habits.Count == 0
            ? 0
            : habits.Count(h => h.IsCompletedToday) * 100.0 / habits.Count;

        HabitStreaks.Clear();
        foreach (var h in habits.OrderByDescending(h => h.CurrentStreak))
        {
            HabitStreaks.Add(new HabitStat { Name = h.Habit.Name, Streak = h.CurrentStreak });
        }

        // ==========================
        // Goals (دايمًا الحالة الحالية، مش متأثرة بالتنقل)
        // ==========================

        var (completedGoals, activeGoals) = await _goalService.GetGoalsSummaryAsync();

        GoalsCompleted = completedGoals;
        GoalsActive = activeGoals;
        GoalsCompletionPercent = await _goalService.GetAverageProgressPercentAsync();

        // ==========================
        // Weekly Trend (آخر 6 أسابيع، ثابتة مش متأثرة بالتنقل)
        // ==========================

        WeeklyTrend.Clear();

        for (int i = 5; i >= 0; i--)
        {
            var wkStart = currentWeekStart.AddDays(-7 * i);
            var wkData = await _statisticsService.GetWeekAsync(wkStart);

            var wkCreated = wkData.Sum(x => x.TasksCreated);
            var wkCompleted = wkData.Sum(x => x.TasksCompleted);

            var pct = wkCreated == 0 ? 0 : Math.Min(100, wkCompleted * 100.0 / wkCreated);

            WeeklyTrend.Add(new WeekTrendStat
            {
                WeekLabel = wkStart.ToString("MMM d"),
                Percent = Math.Round(pct),
                Height = Math.Max(8, (int)(pct * 1.2)),
                IsSelected = (currentWeekStart.AddDays(_weekOffset * 7)) == wkStart
            });
        }
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