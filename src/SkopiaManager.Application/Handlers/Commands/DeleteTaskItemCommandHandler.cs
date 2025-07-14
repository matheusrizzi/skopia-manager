using MediatR;
using SkopiaManager.Application.Commands;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Commands;

public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public DeleteTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<Unit> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(request.TaskItemId, cancellationToken);
        
        if (taskItem == null)
            throw new KeyNotFoundException($"Tarefa não encontrada na base de dados.");

        await _taskItemRepository.DeleteAsync(taskItem, cancellationToken);

        return Unit.Value;
    }
}
