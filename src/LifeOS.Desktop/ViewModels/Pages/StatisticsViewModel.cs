using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LifeOS.Application.Goals;
using LifeOS.Application.Habits;
using LifeOS.Application.Study;
using LifeOS.Application.Tasks;

namespace LifeOS.Desktop.ViewModels.Pages;

public class SubjectStat
{
    public required string Name { get; init; }
    public int Minutes { get; init; }
    public int PercentOfMax { get; init; }
}

public class DayStat
{
    public required string DayLabel { get; init; }
    public int Count { get; init; }
    public int PercentOfMax { get; init; }
}

public class HabitStat
{
    public required string Name { get; init; }
    public int Streak { get; init; }
}

public sealed partial class StatisticsViewModel : ObservableObject
{
    private readonly ITaskService _taskService;
    private readonly IHabitService _habitService;
    private readonly IStudyService _studyService;
    private readonly IGoalService _goalService;
    [ObservableProperty] private double _habitsCompletionPercent;
    [ObservableProperty] private double _goalsCompletionPercent;
    [ObservableProperty] private double _weeklyTaskCompletionPercent;
    
    [ObservableProperty] private int _totalTasksCompleted;
    [ObservableProperty] private int _totalStudyMinutes;
    [ObservableProperty] private int _longestStreak;
    [ObservableProperty] private int _goalsCompleted;
    [ObservableProperty] private int _goalsActive;

    public ObservableCollection<DayStat> WeeklyTasks { get; } = new();
    public ObservableCollection<SubjectStat> SubjectBreakdown { get; } = new();
    public ObservableCollection<HabitStat> HabitStreaks { get; } = new();

    public StatisticsViewModel(
        ITaskService taskService, IHabitService habitService,
        IStudyService studyService, IGoalService goalService)
    {
        _taskService = taskService;
        _habitService = habitService;
        _studyService = studyService;
        _goalService = goalService;

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        // Tasks
        var tasks = await _taskService.GetAllTasksAsync();
        var completed = tasks.Where(t => t.IsCompleted && t.CompletedAt != null).ToList();
        TotalTasksCompleted = completed.Count;

        var today = DateTime.Today;
        var last7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(-6 + i)).ToList();
        var dayCounts = last7Days
            .Select(d => completed.Count(t => t.CompletedAt!.Value.Date == d))
            .ToList();
        var maxDay = Math.Max(1, dayCounts.Max());

        WeeklyTasks.Clear();
        for (var i = 0; i < last7Days.Count; i++)
        {
            WeeklyTasks.Add(new DayStat
            {
                DayLabel = last7Days[i].ToString("ddd"),
                Count = dayCounts[i],
                PercentOfMax = (int)(dayCounts[i] * 100.0 / maxDay)
            });
        }

        // Study
        var summaries = await _studyService.GetSubjectSummariesAsync();
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

        // Habits
        var habits = await _habitService.GetTodayHabitsAsync();
        LongestStreak = habits.Count == 0 ? 0 : habits.Max(h => h.CurrentStreak);

        HabitStreaks.Clear();
        foreach (var h in habits.OrderByDescending(h => h.CurrentStreak))
            HabitStreaks.Add(new HabitStat { Name = h.Habit.Name, Streak = h.CurrentStreak });

        // Goals
        var (goalsCompleted, goalsActive) = await _goalService.GetGoalsSummaryAsync();
        GoalsCompleted = goalsCompleted;
        GoalsActive = goalsActive;

        HabitsCompletionPercent = habits.Count == 0 ? 0 : habits.Count(h => h.IsCompletedToday) * 100.0 / habits.Count;

        var goalsTotal = goalsCompleted + goalsActive;
        GoalsCompletionPercent = goalsTotal == 0 ? 0 : goalsCompleted * 100.0 / goalsTotal;
        
        var weekTotalCreated = tasks.Count(t => t.CreatedAt >= today.AddDays(-6));
        WeeklyTaskCompletionPercent = weekTotalCreated == 0 ? 0 : Math.Min(100, dayCounts.Sum() * 100.0 / weekTotalCreated);
    }
}
