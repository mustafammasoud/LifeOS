using LifeOS.Application.Activity;
using LifeOS.Domain.Goals;

namespace LifeOS.Application.Goals;

public sealed class GoalService : IGoalService
{
    private readonly IGoalRepository _repository;
    private readonly IActivityLogService _activityLog;


    public GoalService(IGoalRepository repository,IActivityLogService activityLog)
    {
        _repository = repository;
        _activityLog = activityLog;
    }

    public async Task<List<GoalProgress>> GetActiveGoalsAsync()
    {
        var goals = await _repository.GetAllAsync();
        return goals
            .Where(g => !g.IsCompleted)
            .Select(g => new GoalProgress { Goal = g })
            .ToList();
    }

    public async Task<Goal> AddManualGoalAsync(string title, DateTime? deadline)
    {
        var goal = new Goal
        {
            Title = title,
            TrackingMode = GoalTrackingMode.Manual,
            Deadline = deadline
        };

        await _repository.AddAsync(goal);
        return goal;
    }

    public async Task<Goal> AddMilestoneGoalAsync(string title, DateTime? deadline, List<string> milestoneTitles)
    {
        var goal = new Goal
        {
            Title = title,
            TrackingMode = GoalTrackingMode.Milestones,
            Deadline = deadline,
            Milestones = milestoneTitles
                .Select((m, i) => new GoalMilestone { Title = m, Order = i })
                .ToList()
        };

        await _repository.AddAsync(goal);
        return goal;
    }

    public async Task SetManualProgressAsync(Guid goalId, int percent)
    {
        var goal = await _repository.GetByIdAsync(goalId);
        if (goal is null) return;

        goal.ManualProgressPercent = Math.Clamp(percent, 0, 100);
        if (goal.ManualProgressPercent == 100) goal.IsCompleted = true;

        await _repository.UpdateAsync(goal);
        await _activityLog.LogAsync("🎯", $"Goal Progress Updated: {goal.Title} → {goal.ManualProgressPercent}%");
    }

    public async Task ToggleMilestoneAsync(Guid goalId, Guid milestoneId)
    {
        var goal = await _repository.GetByIdAsync(goalId);
        if (goal is null) return;

        var milestone = goal.Milestones.FirstOrDefault(m => m.Id == milestoneId);
        if (milestone is null) return;

        milestone.IsCompleted = !milestone.IsCompleted;

        if (goal.Milestones.Count > 0 && goal.Milestones.All(m => m.IsCompleted))
            goal.IsCompleted = true;

        await _repository.UpdateAsync(goal);
        await _activityLog.LogAsync("🎯", $"Goal Progress Updated: {goal.Title} → {goal.ManualProgressPercent}%");
    }
    public Task DeleteGoalAsync(Guid goalId) => _repository.DeleteAsync(goalId);
    public async Task<(int Completed, int Active)> GetGoalsSummaryAsync()
    {
        var goals = await _repository.GetAllAsync();
        return (goals.Count(g => g.IsCompleted), goals.Count(g => !g.IsCompleted));
    }
}
