using LifeOS.Application.Goals;
using LifeOS.Domain.Goals;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Persistence.Repositories;

public class GoalRepository : IGoalRepository
{
    private readonly LifeOSDbContext _context;

    public GoalRepository(LifeOSDbContext context) => _context = context;

    public Task<List<Goal>> GetAllAsync() =>
        _context.Goals.Include(g => g.Milestones).OrderBy(g => g.Deadline).ToListAsync();

    public Task<Goal?> GetByIdAsync(Guid id) =>
        _context.Goals.Include(g => g.Milestones).FirstOrDefaultAsync(g => g.Id == id);

    public async Task AddAsync(Goal goal)
    {
        _context.Goals.Add(goal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Goal goal)
    {
        _context.Goals.Update(goal);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var goal = await GetByIdAsync(id);
        if (goal is null) return;

        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();
    }
}
