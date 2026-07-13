using LifeOS.Domain.Calendar;

namespace LifeOS.Application.Calendar;

public interface ICalendarService
{
    Task<List<CalendarEvent>> GetTodayEventsAsync();
    Task<List<CalendarEvent>> GetEventsForDateAsync(DateOnly date);
    Task<CalendarEvent> AddEventAsync(string title, DateTime start, DateTime end, string? description);
    Task DeleteEventAsync(Guid id);
}
