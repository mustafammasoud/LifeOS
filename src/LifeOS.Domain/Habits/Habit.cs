namespace LifeOS.Domain.Habits;

public class Habit
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public HabitType Type { get; set; } = HabitType.Boolean;
    public int TargetCount { get; set; } = 1; // للـ Boolean بتفضل 1
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
