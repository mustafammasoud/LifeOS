using LifeOS.Domain.Notes;

namespace LifeOS.Application.Notes;

public interface INoteService
{
    Task<List<Note>> GetAllNotesAsync();
    Task<Note> CreateNoteAsync(string title, Guid? subjectId);
    Task UpdateNoteAsync(Guid id, string title, string contentMarkdown, Guid? subjectId);
    Task DeleteNoteAsync(Guid id);
}