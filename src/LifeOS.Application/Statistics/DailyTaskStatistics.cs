namespace LifeOS.Application.Statistics;

public sealed class DailyTaskStatistics
{
    public required DateOnly Date { get; init; }

    public int CreatedTasks { get; init; }

    public int CompletedTasks { get; init; }

    public double CompletionPercent =>
        CreatedTasks == 0
            ? 0
            : (double)CompletedTasks * 100 / CreatedTasks;
}