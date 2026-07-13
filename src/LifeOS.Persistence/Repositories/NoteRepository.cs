using LifeOS.Application.Notes;
using LifeOS.Domain.Notes;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Persistence.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly LifeOSDbContext _context;

    public NoteRepository(LifeOSDbContext context) => _context = context;

    public Task<List<Note>> GetAllAsync() =>
        _context.Notes.OrderByDescending(n => n.UpdatedAt).ToListAsync();

    public Task<Note?> GetByIdAsync(Guid id) =>
        _context.Notes.FirstOrDefaultAsync(n => n.Id == id);

    public async Task AddAsync(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Note note)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var note = await GetByIdAsync(id);
        if (note is null) return;
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
    }
}
