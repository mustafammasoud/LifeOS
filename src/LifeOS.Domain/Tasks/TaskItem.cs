namespace LifeOS.Domain.Tasks;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateOnly StatisticsDate { get; set; }
             = DateOnly.FromDateTime(DateTime.Now);
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsCompleted { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public List<string> Tags { get; set; } = new();
}
