using LifeOS.Application.Activity;
using LifeOS.Domain.Study;

namespace LifeOS.Application.Study;

public sealed class StudyService : IStudyService
{
    private readonly ISubjectRepository _subjects;
    private readonly IPomodoroRepository _sessions;
    private readonly IActivityLogService _activityLog;

    public StudyService(ISubjectRepository subjects, IPomodoroRepository sessions,IActivityLogService activityLog)
    {
        _subjects = subjects;
        _sessions = sessions;
        _activityLog = activityLog;
    }

    public Task<List<Subject>> GetSubjectsAsync() => _subjects.GetAllAsync();

    public async Task<Subject> AddSubjectAsync(string name)
    {
        var subject = new Subject { Name = name };
        await _subjects.AddAsync(subject);
        return subject;
    }

    public async Task<List<SubjectSummary>> GetSubjectSummariesAsync()
    {
        var subjects = await _subjects.GetAllAsync();
        var summaries = new List<SubjectSummary>();

        foreach (var subject in subjects)
        {
            var sessions = await _sessions.GetBySubjectAsync(subject.Id);
            summaries.Add(new SubjectSummary
            {
                SubjectId = subject.Id,
                SubjectName = subject.Name,
                TotalMinutes = sessions.Sum(s => s.DurationMinutes)
            });
        }

        return summaries;
    }

    public Task<List<PomodoroSession>> GetTodaySessionsAsync() => _sessions.GetTodayAsync();

    public async Task<PomodoroSession> LogSessionAsync(Guid? subjectId, int durationMinutes, string? notes)
  {
    var session = new PomodoroSession { SubjectId = subjectId, DurationMinutes = durationMinutes, Notes = notes, CompletedAt = DateTime.UtcNow };
    await _sessions.AddAsync(session);

    await _activityLog.LogAsync("🍅", $"Finished Pomodoro Session ({durationMinutes} min)");

    return session;
  } 

    public async Task<int> GetTodayFocusMinutesAsync()
    {
        var sessions = await _sessions.GetTodayAsync();
        return sessions.Sum(s => s.DurationMinutes);
    }
    public Task DeleteSubjectAsync(Guid subjectId) => _subjects.DeleteAsync(subjectId);
   public async Task<List<SubjectSummary>> GetSubjectSummariesForWeekAsync(DateOnly startDate)
{
    var subjects = await _subjects.GetAllAsync();
    var weekSessions = await _sessions.GetWeekAsync(startDate);

    var summaries = new List<SubjectSummary>();

    foreach (var subject in subjects)
    {
        var minutes = weekSessions
            .Where(s => s.SubjectId == subject.Id)
            .Sum(s => s.DurationMinutes);

        summaries.Add(new SubjectSummary
        {
            SubjectId = subject.Id,
            SubjectName = subject.Name,
            TotalMinutes = minutes
        });
    }

    return summaries;
}
public async Task<Dictionary<int, int>> GetFocusMinutesByHourAsync()
{
    var sessions = await _sessions.GetAllAsync();

    return sessions
        .GroupBy(s => s.StartedAt.ToLocalTime().Hour)
        .ToDictionary(g => g.Key, g => g.Sum(s => s.DurationMinutes));
}
}
