using LifeOS.Domain.Study;

namespace LifeOS.Application.Study;

public interface IStudyService
{
    Task<List<Subject>> GetSubjectsAsync();
    Task<Subject> AddSubjectAsync(string name);
    Task<List<SubjectSummary>> GetSubjectSummariesAsync();
    Task<List<PomodoroSession>> GetTodaySessionsAsync();
    Task<PomodoroSession> LogSessionAsync(Guid? subjectId, int durationMinutes, string? notes);
    Task<int> GetTodayFocusMinutesAsync();
    Task DeleteSubjectAsync(Guid subjectId);
    Task<List<SubjectSummary>> GetSubjectSummariesForWeekAsync(DateOnly startDate);
    Task<Dictionary<int, int>> GetFocusMinutesByHourAsync();
    
}
