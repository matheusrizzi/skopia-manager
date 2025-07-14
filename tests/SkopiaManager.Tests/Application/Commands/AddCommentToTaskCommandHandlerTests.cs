using MediatR;
using NSubstitute;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Handlers.Commands;
using SkopiaManager.Application.Notifications;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Tests.Application.Handlers;

public class AddCommentToTaskCommandHandlerTests
{
    private readonly ITaskItemRepository _taskItemRepository = Substitute.For<ITaskItemRepository>();
    private readonly ICommentRepository _commentRepository = Substitute.For<ICommentRepository>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly AddCommentToTaskCommandHandler _handler;

     public AddCommentToTaskCommandHandlerTests()
    {
        _handler = new AddCommentToTaskCommandHandler(_taskItemRepository, _commentRepository, _mediator);
    }

    [Fact]
    public async Task Handle_Should_AddComment_And_PublishNotification_When_TaskExists()
    {
        // Arrange
        var taskItem = new TaskItem("Título", "Descrição", DateTime.UtcNow, PriorityEnum.Media, TaskStatusEnum.InProgress, 1);
        _taskItemRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(taskItem);

        var command = new AddCommentToTaskCommand(1, "Comentário de teste",42);

        //// Act
        await _handler.Handle(command, CancellationToken.None);

        //// Assert
        await _commentRepository.Received(1)
                                .AddAsync(Arg.Is<Comment>(c =>
                                                                c.TaskItemId == command.TaskItemId &&
                                                                c.Message == command.Message &&
                                                                c.UserId == command.UserId
                                         ), Arg.Any<CancellationToken>());

        await _mediator.Received(1).Publish(Arg.Any<CommentAddedNotification>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_TaskNotFound()
    {
        //// Arrange
        _taskItemRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns((TaskItem)null);

        var command = new AddCommentToTaskCommand(99,"Mensagem", 1);

        //// Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Tarefa não encontrada.", ex.Message);
    }
}
