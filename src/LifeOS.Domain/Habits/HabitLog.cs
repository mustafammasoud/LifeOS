namespace LifeOS.Domain.Habits;

public class HabitLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HabitId { get; set; }
    public DateOnly Date { get; set; }
    public int Count { get; set; } 
}
