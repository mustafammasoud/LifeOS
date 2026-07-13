using LifeOS.Domain.Statistics;

public interface IDailyStatisticsService
{
    Task RegisterTaskCreatedAsync(DateOnly date);

    Task RegisterTaskCompletedAsync(DateOnly date);

    Task RegisterTaskUnCompletedAsync(DateOnly date);

    Task RegisterTaskDeletedAsync(DateOnly date, bool wasCompleted);

    Task<List<DailyStatistics>> GetWeekAsync(DateOnly startDate);
}