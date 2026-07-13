namespace LifeOS.Application.Study;

public class SubjectSummary
{
    public required Guid SubjectId { get; init; }
    public required string SubjectName { get; init; }
    public int TotalMinutes { get; init; }
}
