using LifeOS.Domain.Tasks;

namespace LifeOS.Application.Tasks;

public interface ITaskService
{
    Task<List<TaskItem>> GetTodayTasksAsync();
    Task<List<TaskItem>> GetAllTasksAsync();
    Task<TaskItem> AddTaskAsync(string title, TaskPriority priority = TaskPriority.Medium, DateTime? dueDate = null, List<string>? tags = null);
    Task SetCompletedAsync(Guid id, bool isCompleted);
    Task DeleteTaskAsync(Guid id);
    Task<List<TaskItem>> GetTasksForTodayAsync();
}