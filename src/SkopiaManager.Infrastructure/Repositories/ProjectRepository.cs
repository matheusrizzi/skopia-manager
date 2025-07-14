using Microsoft.EntityFrameworkCore;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;
using SkopiaManager.Infrastructure.Data;

namespace SkopiaManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly SkopiaDbContext _context;

    public ProjectRepository(SkopiaDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _context.Projects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Projects.FindAsync(id);
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasPendingTasksAsync(int projectId)
    {
        return await _context.Tasks
            .AnyAsync(t => t.ProjectId == projectId && t.Status == Domain.Enums.TaskStatusEnum.Pending);
    }

    public async Task<IEnumerable<Project>> GetAllByUserIdAsync(int userId) => await _context.Projects.Where(x=>x.UserId == userId).ToListAsync();
}
