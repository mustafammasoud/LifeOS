using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Application.Tasks;
using LifeOS.Desktop.Services;
using LifeOS.Domain.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Desktop.ViewModels.Pages;

public sealed partial class TasksViewModel : ObservableObject
{
    private readonly ITaskService _taskService;

    public Array PriorityOptions { get; } = Enum.GetValues(typeof(TaskPriority));

    public ObservableCollection<TaskItem> AllTasks { get; } = new();
    public ObservableCollection<TaskItem> FilteredTasks { get; } = new();
    public ObservableCollection<string> AvailableTags { get; } = new();

    [ObservableProperty] private string _newTaskTitle = string.Empty;
    [ObservableProperty] private TaskPriority _newTaskPriority = TaskPriority.Medium;
    [ObservableProperty] private string _newTaskTagsText = string.Empty;
    [ObservableProperty] private string? _selectedTagFilter;
    [ObservableProperty] private int _totalCount;
    [ObservableProperty] private int _completedCount;
    [ObservableProperty] private int _remainingCount;

    private async Task LoadAsync()
    {
        var tasks = await _taskService.GetTasksForTodayAsync();

        var sorted = tasks.OrderByDescending(t => t.Priority).ToList();

        AllTasks.Clear();
        foreach (var t in sorted)
            AllTasks.Add(t);

        AvailableTags.Clear();
        foreach (var tag in sorted.SelectMany(t => t.Tags).Distinct().OrderBy(t => t))
            AvailableTags.Add(tag);

        TotalCount = tasks.Count;
        CompletedCount = tasks.Count(t => t.IsCompleted);
        RemainingCount = tasks.Count(t => !t.IsCompleted);

        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredTasks.Clear();

        var source = string.IsNullOrEmpty(SelectedTagFilter)
            ? AllTasks
            : AllTasks.Where(t => t.Tags.Contains(SelectedTagFilter));

        foreach (var t in source)
            FilteredTasks.Add(t);
    }

    partial void OnSelectedTagFilterChanged(string? value) => ApplyFilter();

    [RelayCommand]
    private void ClearTagFilter() => SelectedTagFilter = null;

    [RelayCommand]
    private void SetTagFilter(string tag) => SelectedTagFilter = tag;

    [RelayCommand]
    private async Task AddTaskAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTaskTitle)) return;

        var tags = NewTaskTagsText
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct()
            .ToList();

        await _taskService.AddTaskAsync(NewTaskTitle, NewTaskPriority, DateTime.Today, tags);
        NewTaskTitle = string.Empty;
        NewTaskTagsText = string.Empty;
        await LoadAsync();
    }

    [RelayCommand]
    private async Task ToggleCompleteAsync(TaskItem task)
    {
        if (task is null) return;

        await _taskService.SetCompletedAsync(task.Id, task.IsCompleted);
        await LoadAsync();

        if (App.Services.GetService<StatisticsViewModel>() is { } stats)
            await stats.LoadAsync();
    }

    private readonly IDialogService _dialogService;

    public TasksViewModel(ITaskService taskService, IDialogService dialogService)
    {
        _taskService = taskService;
        _dialogService = dialogService;
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task DeleteTaskAsync(TaskItem task)
    {
        if (!await _dialogService.ConfirmAsync("Delete Task", $"Delete \"{task.Title}\"?")) return;
        await _taskService.DeleteTaskAsync(task.Id);
        await LoadAsync();
    }
}