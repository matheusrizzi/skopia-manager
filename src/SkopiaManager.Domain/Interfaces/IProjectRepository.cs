using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Domain.Interfaces;

public interface IProjectRepository
{
    Task AddAsync(Project project, CancellationToken cancellationToken = default);
    Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<IEnumerable<Project>> GetAllByUserIdAsync(int userId);
    Task DeleteAsync(Project project);
}