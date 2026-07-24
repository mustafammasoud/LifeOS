using LifeOS.Application.Tasks;
using LifeOS.Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Persistence.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly LifeOSDbContext _context;

    public TaskRepository(LifeOSDbContext context) => _context = context;

    public Task<List<TaskItem>> GetAllAsync() =>
     _context.Tasks
        .OrderBy(t => t.IsCompleted)
        .ThenByDescending(t => t.Priority)
        .ThenBy(t => t.DueDate)
        .ToListAsync();

    public Task<List<TaskItem>> GetDueTodayAsync()
   {
       var today = DateTime.Today;
       var tomorrow = today.AddDays(1);
   
       return _context.Tasks
           .Where(t => !t.IsCompleted && t.DueDate != null && t.DueDate >= today && t.DueDate < tomorrow)
           .OrderByDescending(t => t.Priority)
           .ToListAsync();
   }

    public Task<TaskItem?> GetByIdAsync(Guid id) =>
        _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await GetByIdAsync(id);
        if (task is null) return;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
    public Task<List<TaskItem>> GetByStatisticsDateAsync(DateOnly date)
    {
        return _context.Tasks
            .Where(t => t.StatisticsDate == date)
            .ToListAsync();
    }
    public Task<List<TaskItem>> GetByStatisticsDateRangeAsync(DateOnly start, DateOnly end)
   {
       return _context.Tasks
           .Where(t => t.StatisticsDate >= start && t.StatisticsDate <= end)
           .ToListAsync();
   }
    
}
