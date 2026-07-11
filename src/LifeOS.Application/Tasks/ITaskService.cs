using LifeOS.Domain.Tasks;

namespace LifeOS.Application.Tasks;

public interface ITaskService
{
    Task<List<TaskItem>> GetTodayTasksAsync();
    Task<List<TaskItem>> GetAllTasksAsync();
    Task<TaskItem> AddTaskAsync(string title, TaskPriority priority = TaskPriority.Medium, DateTime? dueDate = null);
    Task CompleteTaskAsync(Guid id);
    Task DeleteTaskAsync(Guid id);
}