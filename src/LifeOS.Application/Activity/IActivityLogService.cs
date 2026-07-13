using LifeOS.Domain.Activity;

namespace LifeOS.Application.Activity;

public interface IActivityLogService
{
    Task<List<ActivityLogEntry>> GetTodayAsync();
    Task LogAsync(string icon, string title, string? detail = null);
}
