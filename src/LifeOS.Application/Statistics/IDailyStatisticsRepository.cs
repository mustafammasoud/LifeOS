using LifeOS.Domain.Statistics;

namespace LifeOS.Application.Statistics;

public interface IDailyStatisticsRepository
{
    Task<DailyStatistics?> GetByDateAsync(DateOnly date);

    Task AddAsync(DailyStatistics statistics);

    Task UpdateAsync(DailyStatistics statistics);

    Task<List<DailyStatistics>> GetWeekAsync(DateOnly startDate);
    Task<List<DailyStatistics>> GetLast7DaysAsync(DateOnly start);
}
