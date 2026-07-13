namespace LifeOS.Application.Statistics;

public interface IStatisticsRepository
{
    Task<List<DailyTaskStatistics>> GetLast7DaysTaskStatisticsAsync(DateOnly weekStart);
}