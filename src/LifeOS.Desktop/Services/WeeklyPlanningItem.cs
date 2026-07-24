using CommunityToolkit.Mvvm.ComponentModel;
using LifeOS.Domain.Tasks;

namespace LifeOS.Desktop.Services;

public sealed partial class WeeklyPlanningItem : ObservableObject
{
    public required string Title { get; init; }
    public required TaskPriority Priority { get; init; }

    [ObservableProperty]
    private bool _isSelected;
}