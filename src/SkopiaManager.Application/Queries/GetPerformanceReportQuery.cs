using MediatR;
using SkopiaManager.Application.Dtos;

namespace SkopiaManager.Application.Queries;

public record GetPerformanceReportQuery(int UserId) : IRequest<List<PerformanceReportDto>>;
