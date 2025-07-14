using Mapster;
using MediatR;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Queries;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Queries;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskItemDto>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public GetTaskByIdQueryHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItemDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskItemRepository.GetByIdAsync(request.TaskId, cancellationToken);
        var result = tasks.Adapt<TaskItemDto>();
        return result;
    }
}
