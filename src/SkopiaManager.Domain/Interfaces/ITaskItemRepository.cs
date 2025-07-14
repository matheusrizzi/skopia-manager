using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Domain.Interfaces;

public interface ITaskItemRepository
{
    Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId);
    Task AddAsync(TaskItem taskItem);
    Task DeleteAsync(TaskItem taskItem, CancellationToken cancellationToken);
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken);
    Task<bool> AnyPendingTasksAsync(int projectId, CancellationToken cancellationToken);
}
