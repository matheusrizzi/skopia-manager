using MediatR;
using SkopiaManager.Application.Dtos;

namespace SkopiaManager.Application.Queries;

public record GetTaskByIdQuery(int TaskId):IRequest<TaskItemDto>
{
}
