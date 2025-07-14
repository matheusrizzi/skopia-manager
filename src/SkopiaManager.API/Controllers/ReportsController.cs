using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkopiaManager.Application.Queries;

namespace SkopiaManager.API.Controllers;

[ApiController]
[Route("api/relatorios")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("desempenho/{userId}")]
    // Supondo que futuramente haverá autenticação com roles
    //[Authorize(Roles = "Gerente")]
    public async Task<IActionResult> GetPerformanceReport(int userId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetPerformanceReportQuery(userId), cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, ex.Message);
        }
    }
}
