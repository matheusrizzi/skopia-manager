
using Microsoft.EntityFrameworkCore;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Interfaces;
using SkopiaManager.Domain.Enums;
using SkopiaManager.Infrastructure.Data;

namespace SkopiaManager.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly SkopiaDbContext _context;

    public ReportRepository(SkopiaDbContext context)
    {
        _context = context;
    }

    public async Task<List<PerformanceReportDto>> GetPerformanceReportAsync(DateTime fromDate, CancellationToken cancellationToken)
    {
        var result = await _context.Users
            .Select(user => new PerformanceReportDto(user.Id, user.Name, _context.Tasks
                    .Count(t =>
                        t.UserId == user.Id &&
                        t.Status == TaskStatusEnum.Completed &&
                        t.DueDate >= fromDate))
            )
            .ToListAsync(cancellationToken);

        return result;
    }
}

