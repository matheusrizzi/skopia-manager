using Mapster;
using MediatR;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Queries;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Queries;

public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, List<TaskItemDto>>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public GetTasksByProjectQueryHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<List<TaskItemDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskItemRepository.GetTasksByProjectIdAsync(request.ProjectId);

        var result = tasks.Adapt<List<TaskItemDto>>();

        return result;
    }
}
