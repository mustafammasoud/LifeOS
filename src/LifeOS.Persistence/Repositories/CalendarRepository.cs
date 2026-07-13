using LifeOS.Application.Calendar;
using LifeOS.Domain.Calendar;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Persistence.Repositories;

public class CalendarRepository : ICalendarRepository
{
    private readonly LifeOSDbContext _context;

    public CalendarRepository(LifeOSDbContext context) => _context = context;

    public Task<List<CalendarEvent>> GetByDateAsync(DateOnly date)
    {
        var start = date.ToDateTime(TimeOnly.MinValue);
        var end = start.AddDays(1);

        return _context.CalendarEvents
            .Where(e => e.StartTime >= start && e.StartTime < end)
            .OrderBy(e => e.StartTime)
            .ToListAsync();
    }

    public Task<List<CalendarEvent>> GetUpcomingAsync(int count) =>
        _context.CalendarEvents
            .Where(e => e.StartTime >= DateTime.Now)
            .OrderBy(e => e.StartTime)
            .Take(count)
            .ToListAsync();

    public async Task AddAsync(CalendarEvent ev)
    {
        _context.CalendarEvents.Add(ev);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var ev = await _context.CalendarEvents.FirstOrDefaultAsync(e => e.Id == id);
        if (ev is null) return;
        _context.CalendarEvents.Remove(ev);
        await _context.SaveChangesAsync();
    }
}
