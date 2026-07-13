using LifeOS.Domain.Study;

namespace LifeOS.Application.Study;

public interface ISubjectRepository
{
    Task<List<Subject>> GetAllAsync();
    Task<Subject?> GetByIdAsync(Guid id);
    Task AddAsync(Subject subject);
    Task DeleteAsync(Guid id);
}
