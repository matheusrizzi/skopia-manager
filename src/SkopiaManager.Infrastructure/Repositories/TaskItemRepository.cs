
using Microsoft.EntityFrameworkCore;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;
using SkopiaManager.Domain.Interfaces;
using SkopiaManager.Infrastructure.Data;

namespace SkopiaManager.Infrastructure.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly SkopiaDbContext _context;

    public TaskItemRepository(SkopiaDbContext context) => _context = context;

    public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Comments) 
            .ToListAsync();
    }

    public async Task AddAsync(TaskItem taskItem)
    {
        await _context.Tasks.AddAsync(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Tasks.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task DeleteAsync(TaskItem taskItem, CancellationToken cancellationToken)
    {
        _context.Tasks.Remove(taskItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken)
    {
        _context.Tasks.Update(taskItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> AnyPendingTasksAsync(int projectId, CancellationToken cancellationToken)
    {
        return await _context.Tasks.AnyAsync(t => t.ProjectId == projectId && t.Status == TaskStatusEnum.Pending, cancellationToken);
    }
}

