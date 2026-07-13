using LifeOS.Application.Habits;
using LifeOS.Domain.Habits;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Persistence.Repositories;

public class HabitRepository : IHabitRepository
{
    private readonly LifeOSDbContext _context;

    public HabitRepository(LifeOSDbContext context) => _context = context;

    public Task<List<Habit>> GetActiveAsync() =>
        _context.Habits.Where(h => !h.IsArchived).ToListAsync();

    public Task<Habit?> GetByIdAsync(Guid id) =>
        _context.Habits.FirstOrDefaultAsync(h => h.Id == id);

    public async Task AddAsync(Habit habit)
    {
        _context.Habits.Add(habit);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
   {
    var habit = await GetByIdAsync(id);
    if (habit is null) return;

    _context.Habits.Remove(habit);
    await _context.SaveChangesAsync();
   }

    public Task<HabitLog?> GetLogAsync(Guid habitId, DateOnly date) =>
        _context.HabitLogs.FirstOrDefaultAsync(l => l.HabitId == habitId && l.Date == date);

    public Task<List<HabitLog>> GetLogsAsync(Guid habitId, DateOnly from, DateOnly to) =>
        _context.HabitLogs
            .Where(l => l.HabitId == habitId && l.Date >= from && l.Date <= to)
            .ToListAsync();

    public async Task UpsertLogAsync(HabitLog log)
    {
        var existing = await GetLogAsync(log.HabitId, log.Date);
        if (existing is null)
            _context.HabitLogs.Add(log);
        else
            existing.Count = log.Count;

        await _context.SaveChangesAsync();
    }
}
