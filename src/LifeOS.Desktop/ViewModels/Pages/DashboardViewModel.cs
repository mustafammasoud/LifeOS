using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Application.Tasks;
using LifeOS.Domain.Tasks;

namespace LifeOS.Desktop.ViewModels.Pages;

public sealed partial class DashboardViewModel : ObservableObject
{
    private readonly ITaskService _taskService;

    [ObservableProperty]
    private string _greeting;

    [ObservableProperty]
    private string _todayLabel;

    [ObservableProperty]
    private string _newTaskTitle = string.Empty;

    public ObservableCollection<TaskItem> TodayTasks { get; } = new();

    public DashboardViewModel(ITaskService taskService)
    {
        _taskService = taskService;
        _greeting = BuildGreeting();
        _todayLabel = DateTime.Now.ToString("dddd, MMMM d");

        _ = LoadTasksAsync();
    }

    private async Task LoadTasksAsync()
    {
        var tasks = await _taskService.GetTodayTasksAsync();
        TodayTasks.Clear();
        foreach (var task in tasks)
            TodayTasks.Add(task);
    }

    [RelayCommand]
    private async Task AddTaskAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTaskTitle)) return;

        await _taskService.AddTaskAsync(NewTaskTitle, TaskPriority.Medium, DateTime.Today);
        NewTaskTitle = string.Empty;
        await LoadTasksAsync();
    }

    [RelayCommand]
    private async Task CompleteTaskAsync(TaskItem task)
    {
        await _taskService.CompleteTaskAsync(task.Id);
        await LoadTasksAsync();
    }

    private static string BuildGreeting()
    {
        var hour = DateTime.Now.Hour;
        return hour switch
        {
            < 12 => "Good morning",
            < 18 => "Good afternoon",
            _ => "Good evening"
        };
    }
}