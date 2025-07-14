using MediatR;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Interfaces;
using SkopiaManager.Application.Queries;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Queries;

public class GetPerformanceReportQueryHandler : IRequestHandler<GetPerformanceReportQuery, List<PerformanceReportDto>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;

    public GetPerformanceReportQueryHandler(IReportRepository reportRepository, IUserRepository userRepository)
    {
        _reportRepository = reportRepository;
        _userRepository = userRepository;
    }

    public async Task<List<PerformanceReportDto>> Handle(GetPerformanceReportQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
            throw new Exception("Usuário não encontrado.");

        if (!string.Equals(user.Role, "Gerente", StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Somente usuários com a função de gerente podem acessar este relatório.");

        var fromDate = DateTime.UtcNow.AddDays(-30);
        return await _reportRepository.GetPerformanceReportAsync(fromDate, cancellationToken);
    }
}

