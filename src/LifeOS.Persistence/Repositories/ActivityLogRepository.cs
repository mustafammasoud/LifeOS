using LifeOS.Application.Activity;
using LifeOS.Domain.Activity;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Persistence.Repositories;

public class ActivityLogRepository : IActivityLogRepository
{
    private readonly LifeOSDbContext _context;

    public ActivityLogRepository(LifeOSDbContext context) => _context = context;

    public Task<List<ActivityLogEntry>> GetTodayAsync()
    {
        var today = DateTime.Today;
        return _context.ActivityLog
            .Where(a => a.Timestamp >= today)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    public async Task AddAsync(ActivityLogEntry entry)
    {
        _context.ActivityLog.Add(entry);
        await _context.SaveChangesAsync();
    }
}
