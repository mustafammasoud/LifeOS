using LifeOS.Domain.Tasks;

namespace LifeOS.Application.Tasks;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync();
    Task<List<TaskItem>> GetDueTodayAsync();
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(Guid id);
    Task<List<TaskItem>> GetByStatisticsDateAsync(DateOnly date);
}
