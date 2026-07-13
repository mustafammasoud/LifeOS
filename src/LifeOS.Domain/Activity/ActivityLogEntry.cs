namespace LifeOS.Domain.Activity;

public class ActivityLogEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Icon { get; set; } = "📌";
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
