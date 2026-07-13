using LifeOS.Domain.Activity;

namespace LifeOS.Application.Activity;

public interface IActivityLogRepository
{
    Task<List<ActivityLogEntry>> GetTodayAsync();
    Task AddAsync(ActivityLogEntry entry);
}
