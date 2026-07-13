using LifeOS.Application.Activity;
using LifeOS.Domain.Habits;

namespace LifeOS.Application.Habits;

public sealed class HabitService : IHabitService
{
    private readonly IHabitRepository _repository;
    private readonly IActivityLogService _activityLog;


    public HabitService(IHabitRepository repository , IActivityLogService activityLog)
    {
        _repository = repository;
        _activityLog = activityLog;
    }

    public async Task<List<HabitProgress>> GetTodayHabitsAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var habits = await _repository.GetActiveAsync();
        var result = new List<HabitProgress>();

        foreach (var habit in habits)
        {
            var log = await _repository.GetLogAsync(habit.Id, today);
            var streak = await CalculateStreakAsync(habit, today);

            result.Add(new HabitProgress
            {
                Habit = habit,
                TodayCount = log?.Count ?? 0,
                CurrentStreak = streak
            });
        }

        return result;
    }

    public async Task<Habit> AddHabitAsync(string name, HabitType type, int targetCount = 1)
    {
        var habit = new Habit { Name = name, Type = type, TargetCount = targetCount };
        await _repository.AddAsync(habit);
        return habit;
    }

    public async Task LogProgressAsync(Guid habitId, int increment = 1)
  {
    var habit = await _repository.GetByIdAsync(habitId);
    var today = DateOnly.FromDateTime(DateTime.Today);
    var log = await _repository.GetLogAsync(habitId, today) ?? new HabitLog { HabitId = habitId, Date = today };

    log.Count += increment;
    await _repository.UpsertLogAsync(log);

    if (habit != null)
        await _activityLog.LogAsync("💧", $"Completed Habit: {habit.Name}");
  }

    private async Task<int> CalculateStreakAsync(Habit habit, DateOnly today)
    {
        var logs = await _repository.GetLogsAsync(habit.Id, today.AddDays(-60), today);
        var logsByDate = logs.ToDictionary(l => l.Date);

        var streak = 0;
        var date = today;

        while (logsByDate.TryGetValue(date, out var log) && log.Count >= habit.TargetCount)
        {
            streak++;
            date = date.AddDays(-1);
        }

        return streak;
    }

    public Task DeleteHabitAsync(Guid habitId) => _repository.DeleteAsync(habitId);
}
