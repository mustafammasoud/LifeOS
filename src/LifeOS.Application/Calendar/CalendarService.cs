using LifeOS.Domain.Calendar;

namespace LifeOS.Application.Calendar;

public sealed class CalendarService : ICalendarService
{
    private readonly ICalendarRepository _repository;

    public CalendarService(ICalendarRepository repository) => _repository = repository;

    public Task<List<CalendarEvent>> GetTodayEventsAsync() =>
        _repository.GetByDateAsync(DateOnly.FromDateTime(DateTime.Today));

    public Task<List<CalendarEvent>> GetEventsForDateAsync(DateOnly date) =>
        _repository.GetByDateAsync(date);

    public async Task<CalendarEvent> AddEventAsync(string title, DateTime start, DateTime end, string? description)
    {
        var ev = new CalendarEvent { Title = title, StartTime = start, EndTime = end, Description = description };
        await _repository.AddAsync(ev);
        return ev;
    }

    public Task DeleteEventAsync(Guid id) => _repository.DeleteAsync(id);
}
