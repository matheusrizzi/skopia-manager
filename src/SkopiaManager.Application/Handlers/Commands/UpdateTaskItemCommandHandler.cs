using MediatR;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Notifications;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Commands;

public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, TaskItem>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IMediator _mediator;
    private int _userId;
    private CancellationToken _cancellationToken;

    public UpdateTaskItemCommandHandler(ITaskItemRepository taskItemRepository, IMediator mediator)
    {
        _taskItemRepository = taskItemRepository;
        _mediator = mediator;
    }

    public async Task<TaskItem> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(request.TaskItemId, cancellationToken);
        
        if (taskItem == null)
            throw new KeyNotFoundException($"Não foi possivel encontrar a tarefa.");
        
        _userId = request.userId;
        _cancellationToken = cancellationToken;

        if (taskItem.Title != request.Title && !string.IsNullOrEmpty(request.Title))
            await CallNotificationAsync(taskId: taskItem.Id, description: $"Alterado titulo da tarefa de {taskItem.Title} para {request.Title}");

        if (taskItem.Description != request.Description && !string.IsNullOrEmpty(request.Description))
            await CallNotificationAsync(taskId: taskItem.Id, description: $"Alterada descrição da tarefa de {taskItem.Description} para {request.Description}");

        if (taskItem.DueDate != request.DueDate && request.DueDate.HasValue)
            await CallNotificationAsync(taskId: taskItem.Id, description: $"Alterada data de vencimento da tarefa de {taskItem.DueDate} para {request.DueDate}");

        if (taskItem.Status != request.Status && request.Status.HasValue)
            await CallNotificationAsync(taskId: taskItem.Id, description: $"Alterado status da tarefa de {taskItem.Status} para {request.Status}");

        taskItem.UpdateTitle(request.Title, request.userId)
                .UpdateDescription(request.Description, request.userId)
                .UpdateStatus(request.Status, request.userId)
                .UpdateDueDate(request.DueDate, request.userId);

        // A prioridade NÃO pode ser alterada conforme regra de negócio

        await _taskItemRepository.UpdateAsync(taskItem, cancellationToken);

        return taskItem;
    }

    private async Task CallNotificationAsync(int taskId, string description)
        => await _mediator.Publish(new TaskUpdatedNotification(taskId, description, _userId), _cancellationToken);
    
}
