using LifeOS.Application.Activity;
using LifeOS.Domain.Tasks;
using LifeOS.Application.Statistics;

namespace LifeOS.Application.Tasks;

public sealed class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IActivityLogService _activityLog;
    private readonly IDailyStatisticsService _dailyStatistics;

   public TaskService(
    ITaskRepository repository,
    IActivityLogService activityLog,
    IDailyStatisticsService dailyStatistics)
    {
        _repository = repository;
        _activityLog = activityLog;
        _dailyStatistics = dailyStatistics;
    }

    public Task<List<TaskItem>> GetTodayTasksAsync() => _repository.GetDueTodayAsync();

    public Task<List<TaskItem>> GetAllTasksAsync() => _repository.GetAllAsync();

    public async Task<TaskItem> AddTaskAsync(string title, TaskPriority priority = TaskPriority.Medium, DateTime? dueDate = null)
    {
        var task = new TaskItem
        {
            Title = title,
            Priority = priority,
            DueDate = dueDate,
            StatisticsDate = DateOnly.FromDateTime(DateTime.Now)
        };

        await _repository.AddAsync(task);
        await _dailyStatistics.RegisterTaskCreatedAsync(task.StatisticsDate);
        return task;
    }

   public async Task SetCompletedAsync(Guid id, bool isCompleted)
  {
      var task = await _repository.GetByIdAsync(id);
      if (task is null)
          return;
  
      task.IsCompleted = isCompleted;
      task.CompletedAt = isCompleted ? DateTime.UtcNow : null;
  
      await _repository.UpdateAsync(task);
  
      var day = task.StatisticsDate;   
  
      if (isCompleted)
          await _dailyStatistics.RegisterTaskCompletedAsync(day);
      else
          await _dailyStatistics.RegisterTaskUnCompletedAsync(day);
  
      if (isCompleted)
          await _activityLog.LogAsync("✅", $"Completed \"{task.Title}\"");
  }


   public async Task DeleteTaskAsync(Guid id)
  {
      var task = await _repository.GetByIdAsync(id);
  
      if (task is null)
          return;
  
      await _repository.DeleteAsync(id);
  
      await _dailyStatistics.RegisterTaskDeletedAsync(task.StatisticsDate, task.IsCompleted); 
  }
  public Task<List<TaskItem>> GetTasksForTodayAsync()
    => _repository.GetByStatisticsDateAsync(DateOnly.FromDateTime(DateTime.Now));
}
