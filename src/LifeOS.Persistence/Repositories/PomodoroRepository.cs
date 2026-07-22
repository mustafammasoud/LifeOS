using LifeOS.Application.Study;
using LifeOS.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Persistence.Repositories;

public class PomodoroRepository : IPomodoroRepository
{
    private readonly LifeOSDbContext _context;

    public PomodoroRepository(LifeOSDbContext context) => _context = context;

    public Task<List<PomodoroSession>> GetTodayAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return _context.PomodoroSessions
            .Where(p => p.StartedAt >= today && p.StartedAt < tomorrow)
            .OrderByDescending(p => p.StartedAt)
            .ToListAsync();
    }

    public Task<List<PomodoroSession>> GetBySubjectAsync(Guid subjectId) =>
        _context.PomodoroSessions.Where(p => p.SubjectId == subjectId).ToListAsync();

    public async Task AddAsync(PomodoroSession session)
    {
        _context.PomodoroSessions.Add(session);
        await _context.SaveChangesAsync();
    }
       public Task<List<PomodoroSession>> GetWeekAsync(DateOnly startDate)
   {
       var start = startDate.ToDateTime(TimeOnly.MinValue);
       var end = start.AddDays(7);
   
       return _context.PomodoroSessions
           .Where(p => p.StartedAt >= start && p.StartedAt < end)
           .ToListAsync();
   }
}
