using LifeOS.Domain.Statistics;

namespace LifeOS.Application.Statistics;

public sealed class DailyStatisticsService : IDailyStatisticsService
{
    private readonly IDailyStatisticsRepository _repository;

    public DailyStatisticsService(IDailyStatisticsRepository repository)
    {
        _repository = repository;
    }
    private async Task<DailyStatistics> GetOrCreateAsync(DateOnly date)
  {
      var statistics = await _repository.GetByDateAsync(date);
  
      if (statistics is not null)
          return statistics;
  
      statistics = new DailyStatistics
      {
          Date = date,
          TasksCreated = 0,
          TasksCompleted = 0,
          StudyMinutes = 0,
          HabitsCompleted = 0,
          GoalsCompleted = 0
      };
  
      await _repository.AddAsync(statistics);
  
      return statistics;
  }

    public async Task RegisterTaskCreatedAsync(DateOnly date)
    {
        var statistics = await GetOrCreateAsync(date);
    
        statistics.TasksCreated++;
    
        await _repository.UpdateAsync(statistics);
    }
    public async Task RegisterTaskCompletedAsync(DateOnly date)
    {
        var statistics = await GetOrCreateAsync(date);
    
        statistics.TasksCompleted++;
    
        await _repository.UpdateAsync(statistics);
    }

   public async Task RegisterTaskUnCompletedAsync(DateOnly date)
   {
       var statistics = await GetOrCreateAsync(date);
   
       if (statistics.TasksCompleted > 0)
           statistics.TasksCompleted--;
   
       await _repository.UpdateAsync(statistics);
   }

    public Task<List<DailyStatistics>> GetWeekAsync(DateOnly startDate)
    {
        return _repository.GetWeekAsync(startDate);
    }
    public async Task RegisterTaskDeletedAsync(DateOnly date, bool wasCompleted)
{
    var stats = await GetOrCreateAsync(date);

    if (stats.TasksCreated > 0)
        stats.TasksCreated--;

    if (wasCompleted && stats.TasksCompleted > 0)
        stats.TasksCompleted--;

    await _repository.UpdateAsync(stats);
}
}