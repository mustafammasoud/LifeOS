using LifeOS.Application.Statistics;
using LifeOS.Domain.Statistics;
using Microsoft.EntityFrameworkCore;
namespace LifeOS.Persistence.Repositories;

public sealed class DailyStatisticsRepository : IDailyStatisticsRepository
{
    private readonly LifeOSDbContext _context;

    public DailyStatisticsRepository(LifeOSDbContext context)
    {
        _context = context;
    }

    public Task<DailyStatistics?> GetByDateAsync(DateOnly date)
        => _context.DailyStatistics
            .FirstOrDefaultAsync(x => x.Date == date);

    public async Task AddAsync(DailyStatistics statistics)
    {
        _context.DailyStatistics.Add(statistics);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DailyStatistics statistics)
    {
        _context.DailyStatistics.Update(statistics);
        await _context.SaveChangesAsync();
    }

    public Task<List<DailyStatistics>> GetWeekAsync(DateOnly startDate)
    {
        var end = startDate.AddDays(6);

        return _context.DailyStatistics
            .Where(x => x.Date >= startDate && x.Date <= end)
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

public async Task<List<DailyStatistics>> GetLast7DaysAsync(DateOnly start)
{
    var end = start.AddDays(6);

    var data = await _context.DailyStatistics
        .Where(x => x.Date >= start && x.Date <= end)
        .OrderBy(x => x.Date)
        .ToListAsync();

    var result = new List<DailyStatistics>();

    for (int i = 0; i < 7; i++)
    {
        var date = start.AddDays(i);

        var stat = data.FirstOrDefault(x => x.Date == date);

        result.Add(stat ?? new DailyStatistics
        {
            Date = date,
            TasksCreated = 0,
            TasksCompleted = 0,
            StudyMinutes = 0,
            HabitsCompleted = 0,
            GoalsCompleted = 0
        });
    }

    return result;}

}
