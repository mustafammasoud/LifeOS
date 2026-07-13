using LifeOS.Application.Study;
using LifeOS.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace LifeOS.Persistence.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly LifeOSDbContext _context;

    public SubjectRepository(LifeOSDbContext context) => _context = context;

    public Task<List<Subject>> GetAllAsync() =>
        _context.Subjects.OrderBy(s => s.Name).ToListAsync();

    public Task<Subject?> GetByIdAsync(Guid id) =>
        _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);

    public async Task AddAsync(Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var subject = await GetByIdAsync(id);
        if (subject is null) return;
        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();
    }
}
