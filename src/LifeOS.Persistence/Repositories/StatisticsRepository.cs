using LifeOS.Application.Statistics;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Persistence.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly LifeOSDbContext _context;

    public StatisticsRepository(LifeOSDbContext context)
    {
        _context = context;
    }

    public async Task<List<DailyTaskStatistics>> GetLast7DaysTaskStatisticsAsync(DateOnly weekStart)
{
    var weekEnd = weekStart.AddDays(6);

    var tasks = await _context.Tasks
        .Where(t =>
            t.StatisticsDate >= weekStart &&
            t.StatisticsDate <= weekEnd)
           .ToListAsync();

    var result = new List<DailyTaskStatistics>();

    for (int i = 0; i < 7; i++)
    {
        var day = weekStart.AddDays(i);

        var dayTasks = tasks.Where(t => t.StatisticsDate == day).ToList();

        result.Add(new DailyTaskStatistics
        {
            Date = day,
            CreatedTasks = dayTasks.Count,
            CompletedTasks = dayTasks.Count(t => t.IsCompleted)
        });
    }

    return result;
}
}
