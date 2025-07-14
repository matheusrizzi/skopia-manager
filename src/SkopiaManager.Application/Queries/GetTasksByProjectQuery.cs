using MediatR;
using SkopiaManager.Application.Dtos;

namespace SkopiaManager.Application.Queries;

public class GetTasksByProjectQuery : IRequest<List<TaskItemDto>>
{
    public int ProjectId { get; set; }

    public GetTasksByProjectQuery(int projectId)
    {
        ProjectId = projectId;
    }
}
