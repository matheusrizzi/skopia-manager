using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace SkopiaManager.API.Controllers;

[ApiController]
[Route("api/projeto")]
[SwaggerTag("Projeto (UserId 1 = Gerente, 2 = Desenvolvedor) Criados por migration")]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cria um novo projeto com tarefas.
    /// </summary>
    [HttpPost("CriarProjeto")]
    public async Task<ActionResult<ProjectDto>> CreateProject ([FromBody] CreateProjectCommand command)
    {
        if (command.UserId == 0)
            return NotFound("Informe um usuário valido.");

        var project = await _mediator.Send(command);
        return Ok("Projeto criado com sucesso!");
    }

    /// <summary>
    /// Retorna todos os projetos.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
    {
        var projects = await _mediator.Send(new GetAllProjectsQuery(null));
        return Ok(projects);
    }

    /// <summary>
    /// Retorna todos os projetos por usuario
    /// </summary>
    [HttpGet("ObterProjetosPorUsuario/{userId}")]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjectsByUser(int userId)
    {
        var projects = await _mediator.Send(new GetAllProjectsQuery(userId));
        return Ok(projects);
    }

    /// <summary>
    /// Retorna todas as tarefas de um projeto.
    /// </summary>
    [HttpGet("{projectId}/tarefas")]
    public async Task<IActionResult> GetTasksByProject(int projectId)
    {
        var query = new GetTasksByProjectQuery(projectId);
        var tasks = await _mediator.Send(query);
        return Ok(tasks);
    }

    /// <summary>
    /// Remove um projeto se não houver tarefas pendentes.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoverProjeto(int id)
    {
        try
        {
            await _mediator.Send(new DeleteProjectCommand(id));
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno no servidor." });
        }

    }
}


