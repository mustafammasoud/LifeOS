using LifeOS.Domain.Goals;

namespace LifeOS.Application.Goals;

public interface IGoalRepository
{
    Task<List<Goal>> GetAllAsync();
    Task<Goal?> GetByIdAsync(Guid id);
    Task AddAsync(Goal goal);
    Task UpdateAsync(Goal goal);
    Task DeleteAsync(Guid id);
}
