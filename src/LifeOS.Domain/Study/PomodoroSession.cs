namespace LifeOS.Domain.Study;

public class PomodoroSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? SubjectId { get; set; }
    public int DurationMinutes { get; set; } = 25;
    public string? Notes { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
