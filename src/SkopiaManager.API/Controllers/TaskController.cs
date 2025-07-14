using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace SkopiaManager.API.Controllers;

[ApiController]
[Route("api/tarefa")]
[SwaggerTag("Tarefa (Pending=0, InProgress=1, Completed=2)")]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> Create([FromBody] CreateTaskItemCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemDto>> GetById(int id)
    {
        var taskItem = await _mediator.Send(new GetTaskByIdQuery(id));
        return Ok(taskItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteTaskItemCommand(id));
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemCommand command)
    {
        if (id != command.TaskItemId)
            return BadRequest("Identificação da tarefa não confere.");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{taskItemId}/comentarios")]
    public async Task<IActionResult> AddCommentToTask(int taskItemId, [FromBody] AddCommentToTaskCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Comentário adicionado com sucesso" });
    }
}