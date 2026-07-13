using LifeOS.Application.Activity;
using LifeOS.Domain.Tasks;

namespace LifeOS.Application.Tasks;

public sealed class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IActivityLogService _activityLog;

    public TaskService(ITaskRepository repository, IActivityLogService activityLog)
    {
        _repository = repository;
        _activityLog = activityLog;
    }

    public Task<List<TaskItem>> GetTodayTasksAsync() => _repository.GetDueTodayAsync();

    public Task<List<TaskItem>> GetAllTasksAsync() => _repository.GetAllAsync();

    public async Task<TaskItem> AddTaskAsync(string title, TaskPriority priority = TaskPriority.Medium, DateTime? dueDate = null)
    {
        var task = new TaskItem
        {
            Title = title,
            Priority = priority,
            DueDate = dueDate
        };

        await _repository.AddAsync(task);
        return task;
    }

    public async Task CompleteTaskAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null) return;

        task.IsCompleted = true;
        task.CompletedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(task);

        await _activityLog.LogAsync("✅", $"Completed \"{task.Title}\"");
    }

    public Task DeleteTaskAsync(Guid id) => _repository.DeleteAsync(id);
}
