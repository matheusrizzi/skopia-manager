using System.Diagnostics.CodeAnalysis;

namespace SkopiaManager.Application.Dtos;

[ExcludeFromCodeCoverage]
public record PerformanceReportDto(int UserId, string UserName, double AverageCompletedTasks)
{
}

