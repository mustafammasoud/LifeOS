using LifeOS.Domain.Habits;

namespace LifeOS.Application.Habits;

public interface IHabitRepository
{
    Task<List<Habit>> GetActiveAsync();
    Task<Habit?> GetByIdAsync(Guid id);
    Task AddAsync(Habit habit);
    Task<HabitLog?> GetLogAsync(Guid habitId, DateOnly date);
    Task<List<HabitLog>> GetLogsAsync(Guid habitId, DateOnly from, DateOnly to);
    Task UpsertLogAsync(HabitLog log);

    Task DeleteAsync(Guid id);
    
}
