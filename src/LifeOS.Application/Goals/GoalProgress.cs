using LifeOS.Domain.Goals;
namespace LifeOS.Application.Goals;
public class GoalProgress
{
    public required Goal Goal { get; init; }

    public int ProgressPercent => Goal.TrackingMode == GoalTrackingMode.Manual
        ? Goal.ManualProgressPercent
        : CalculateFromMilestones();

    // property مؤقتة للـ Slider بس، تتهيأ بنفس قيمة ProgressPercent
    private int? _editPercent;
    public int EditPercent
    {
        get => _editPercent ?? ProgressPercent;
        set => _editPercent = value;
    }

    private int CalculateFromMilestones()
    {
        if (Goal.Milestones.Count == 0) return 0;
        var completed = Goal.Milestones.Count(m => m.IsCompleted);
        return (int)Math.Round(completed * 100.0 / Goal.Milestones.Count);
    }
}