using LifeOS.Domain.Study;

namespace LifeOS.Application.Study;

public interface IPomodoroRepository
{
    Task<List<PomodoroSession>> GetTodayAsync();
    Task<List<PomodoroSession>> GetBySubjectAsync(Guid subjectId);
    Task AddAsync(PomodoroSession session);
    Task<List<PomodoroSession>> GetWeekAsync(DateOnly startDate); 
    Task<List<PomodoroSession>> GetAllAsync(); 
}
