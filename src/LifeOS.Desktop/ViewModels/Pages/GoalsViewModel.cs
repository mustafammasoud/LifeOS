using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Application.Goals;
using LifeOS.Desktop.Services;
using LifeOS.Domain.Goals;

namespace LifeOS.Desktop.ViewModels.Pages;

public sealed partial class GoalsViewModel : ObservableObject
{
    private readonly IGoalService _goalService;

    public ObservableCollection<GoalProgress> Goals { get; } = new();

    [ObservableProperty]
    private string _newGoalTitle = string.Empty;

    [ObservableProperty]
    private bool _useMilestones;

    [ObservableProperty]
    private string _milestonesInput = string.Empty; // خطوات مفصولة بفاصلة

    [ObservableProperty]
    private DateTimeOffset? _deadline;


    private async Task LoadAsync()
    {
        var goals = await _goalService.GetActiveGoalsAsync();
        Goals.Clear();
        foreach (var g in goals)
            Goals.Add(g);
    }

    [RelayCommand]
    private async Task AddGoalAsync()
    {
        if (string.IsNullOrWhiteSpace(NewGoalTitle)) return;

        var deadline = Deadline?.DateTime;

        if (UseMilestones)
        {
            var milestones = MilestonesInput
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            await _goalService.AddMilestoneGoalAsync(NewGoalTitle, deadline, milestones);
        }
        else
        {
            await _goalService.AddManualGoalAsync(NewGoalTitle, deadline);
        }

        NewGoalTitle = string.Empty;
        MilestonesInput = string.Empty;
        Deadline = null;
        await LoadAsync();
    }

    [RelayCommand]
   private async Task SetProgressAsync(GoalProgress progress)
   {
       await _goalService.SetManualProgressAsync(progress.Goal.Id, progress.EditPercent);
       await LoadAsync();
   }
 private async Task SetProgressAsync((GoalProgress Goal, int Percent) args)
    {
        await _goalService.SetManualProgressAsync(args.Goal.Goal.Id, args.Percent);
        await LoadAsync();
    }

    [RelayCommand]
    private async Task ToggleMilestoneAsync((GoalProgress Goal, GoalMilestone Milestone) args)
    {
        await _goalService.ToggleMilestoneAsync(args.Goal.Goal.Id, args.Milestone.Id);
        await LoadAsync();
    }
    private readonly IDialogService _dialogService;

    public GoalsViewModel(IGoalService goalService, IDialogService dialogService)
    {
        _goalService = goalService;
        _dialogService = dialogService;
        _ = LoadAsync();
    }
    
    [RelayCommand]
    private async Task DeleteGoalAsync(GoalProgress progress)
    {
        if (!await _dialogService.ConfirmAsync("Delete Goal", $"Delete \"{progress.Goal.Title}\"?")) return;
        await _goalService.DeleteGoalAsync(progress.Goal.Id);
        await LoadAsync();
    }
    
}
