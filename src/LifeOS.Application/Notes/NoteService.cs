using LifeOS.Application.Activity;
using LifeOS.Domain.Notes;

namespace LifeOS.Application.Notes;

public sealed class NoteService : INoteService
{
    private readonly INoteRepository _repository;
    private readonly IActivityLogService _activityLog;


    public NoteService(INoteRepository repository , IActivityLogService activityLog)
    {
        _repository = repository ;
        _activityLog = activityLog;
    }

    public Task<List<Note>> GetAllNotesAsync() => _repository.GetAllAsync();

    public async Task<Note> CreateNoteAsync(string title, Guid? subjectId)
   {
    var note = new Note { Title = title, SubjectId = subjectId };
    await _repository.AddAsync(note);
    await _activityLog.LogAsync("📝", $"Created Note \"{title}\"");
    return note;
   }
    public async Task UpdateNoteAsync(Guid id, string title, string contentMarkdown, Guid? subjectId)
    {
        var note = await _repository.GetByIdAsync(id);
        if (note is null) return;

        note.Title = title;
        note.ContentMarkdown = contentMarkdown;
        note.SubjectId = subjectId;
        note.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(note);
    }

    public Task DeleteNoteAsync(Guid id) => _repository.DeleteAsync(id);
}
