using MediatR;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Notifications;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Commands;

public class AddCommentToTaskCommandHandler : IRequestHandler<AddCommentToTaskCommand>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IMediator _mediator;

    public AddCommentToTaskCommandHandler(
        ITaskItemRepository taskItemRepository,
        ICommentRepository commentRepository,
        IMediator mediator)
    {
        _taskItemRepository = taskItemRepository;
        _commentRepository = commentRepository;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(AddCommentToTaskCommand request, CancellationToken cancellationToken)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(request.TaskItemId, cancellationToken);
        if (taskItem == null)
            throw new Exception("Tarefa não encontrada.");

        var comment = new Comment
        {
            TaskItemId = request.TaskItemId,
            Message = request.Message,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddAsync(comment, cancellationToken);
        await _mediator.Publish(new CommentAddedNotification(taskItem.Id, $"Comentário adicionado: {request.Message}", request.UserId));

        return Unit.Value;
    }
}
