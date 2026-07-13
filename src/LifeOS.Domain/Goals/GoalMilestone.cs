namespace LifeOS.Domain.Goals;

public class GoalMilestone
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid GoalId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int Order { get; set; }
}
