using LifeOS.Domain.Habits;

namespace LifeOS.Application.Habits;

public interface IHabitService
{
    Task<List<HabitProgress>> GetTodayHabitsAsync();
    Task<Habit> AddHabitAsync(string name, HabitType type, int targetCount = 1);
    Task LogProgressAsync(Guid habitId, int increment = 1);
    Task DeleteHabitAsync(Guid habitId);
}
