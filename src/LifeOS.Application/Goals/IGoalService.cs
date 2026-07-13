using LifeOS.Domain.Goals;

namespace LifeOS.Application.Goals;

public interface IGoalService
{
    Task<List<GoalProgress>> GetActiveGoalsAsync();
    Task<Goal> AddManualGoalAsync(string title, DateTime? deadline);
    Task<Goal> AddMilestoneGoalAsync(string title, DateTime? deadline, List<string> milestoneTitles);
    Task SetManualProgressAsync(Guid goalId, int percent);
    Task ToggleMilestoneAsync(Guid goalId, Guid milestoneId);
    Task DeleteGoalAsync(Guid goalId);
    Task<(int Completed, int Active)> GetGoalsSummaryAsync();
    
}
