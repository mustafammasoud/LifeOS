using LifeOS.Domain.Activity;

namespace LifeOS.Application.Activity;

public sealed class ActivityLogService : IActivityLogService
{
    private readonly IActivityLogRepository _repository;

    public ActivityLogService(IActivityLogRepository repository) => _repository = repository;

    public Task<List<ActivityLogEntry>> GetTodayAsync() => _repository.GetTodayAsync();

    public Task LogAsync(string icon, string title, string? detail = null) =>
        _repository.AddAsync(new ActivityLogEntry { Icon = icon, Title = title, Detail = detail });
}
