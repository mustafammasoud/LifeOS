using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Application.Tasks;
using LifeOS.Desktop.Services;
using LifeOS.Domain.Tasks;

namespace LifeOS.Desktop.ViewModels.Pages;

public sealed partial class TasksViewModel : ObservableObject
{
    private readonly ITaskService _taskService;

    public Array PriorityOptions { get; } = Enum.GetValues(typeof(TaskPriority));

    public ObservableCollection<TaskItem> AllTasks { get; } = new();

    [ObservableProperty] private string _newTaskTitle = string.Empty;
    [ObservableProperty] private TaskPriority _newTaskPriority = TaskPriority.Medium;


    private async Task LoadAsync()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        AllTasks.Clear();
        foreach (var t in tasks)
            AllTasks.Add(t);
    }

    [RelayCommand]
    private async Task AddTaskAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTaskTitle)) return;

        await _taskService.AddTaskAsync(NewTaskTitle, NewTaskPriority);
        NewTaskTitle = string.Empty;
        await LoadAsync();
    }

    [RelayCommand]
    private async Task ToggleCompleteAsync(TaskItem task)
    {
        if (!task.IsCompleted)
            await _taskService.CompleteTaskAsync(task.Id);
        await LoadAsync();
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
