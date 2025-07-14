using MediatR;
using SkopiaManager.Application.Commands;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskItemRepository _taskItemRepository;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository, ITaskItemRepository taskItemRepository)
    {
        _projectRepository = projectRepository;
        _taskItemRepository = taskItemRepository;
    }

    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            throw new KeyNotFoundException($"Projeto {request.ProjectId} não encontrado.");

        var pendingTasks = await _taskItemRepository.AnyPendingTasksAsync(request.ProjectId, cancellationToken);
        if (pendingTasks)
            throw new InvalidOperationException("Não é possivel excluir um projeto com tarefas pendentes. Por favor complete ou remova as tasks primeiro.");

        await _projectRepository.DeleteAsync(project);

        return Unit.Value;
    }
}
