using LifeOS.Domain.Habits;

namespace LifeOS.Application.Habits;

public class HabitProgress
{
    public required Habit Habit { get; init; }
    public int TodayCount { get; init; }
    public bool IsCompletedToday => TodayCount >= Habit.TargetCount;
    public int CurrentStreak { get; init; }
}
