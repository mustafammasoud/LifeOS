using LifeOS.Domain.Calendar;

namespace LifeOS.Application.Calendar;

public interface ICalendarRepository
{
    Task<List<CalendarEvent>> GetByDateAsync(DateOnly date);
    Task<List<CalendarEvent>> GetUpcomingAsync(int count);
    Task AddAsync(CalendarEvent ev);
    Task DeleteAsync(Guid id);
}
