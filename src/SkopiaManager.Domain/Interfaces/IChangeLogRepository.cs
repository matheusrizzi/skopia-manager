using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Domain.Interfaces;

public interface IChangeLogRepository
{
    Task AddAsync(ChangeLog changeLog, CancellationToken cancellationToken);
    Task<IEnumerable<ChangeLog>> GetByTaskIdAsync(int taskItemId, CancellationToken cancellationToken);
}