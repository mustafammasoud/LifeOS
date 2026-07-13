namespace LifeOS.Domain.Statistics;

public class DailyStatistics
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateOnly Date { get; set; }

    public int TasksCreated { get; set; }

    public int TasksCompleted { get; set; }

    public int StudyMinutes { get; set; }

    public int HabitsCompleted { get; set; }

    public int GoalsCompleted { get; set; }
}

