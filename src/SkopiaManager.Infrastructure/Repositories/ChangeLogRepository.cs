using Microsoft.EntityFrameworkCore;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;
using SkopiaManager.Infrastructure.Data;

namespace SkopiaManager.Infrastructure.Repositories;

public class ChangeLogRepository : IChangeLogRepository
{
    private readonly SkopiaDbContext _context;

    public ChangeLogRepository(SkopiaDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChangeLog changeLog, CancellationToken cancellationToken)
    {
        await _context.ChangeLogs.AddAsync(changeLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ChangeLog>> GetByTaskIdAsync(int taskItemId, CancellationToken cancellationToken)
    {
        return await _context.ChangeLogs
            .Where(log => log.TaskItemId == taskItemId)
            .OrderByDescending(log => log.ModifiedAt)
            .ToListAsync(cancellationToken);
    }
}