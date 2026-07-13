namespace LifeOS.Domain.Goals;

public class Goal
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public GoalTrackingMode TrackingMode { get; set; } = GoalTrackingMode.Manual;
    public int ManualProgressPercent { get; set; } // يستخدم بس لو Manual
    public DateTime? Deadline { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<GoalMilestone> Milestones { get; set; } = new();
}
