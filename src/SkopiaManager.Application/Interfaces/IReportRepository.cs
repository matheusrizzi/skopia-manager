using SkopiaManager.Application.Dtos;

namespace SkopiaManager.Application.Interfaces;

public interface IReportRepository
{
    Task<List<PerformanceReportDto>> GetPerformanceReportAsync(DateTime fromDate, CancellationToken cancellationToken);
}

