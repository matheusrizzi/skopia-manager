using FluentAssertions;
using MediatR;
using NSubstitute;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Handlers.Commands;
using SkopiaManager.Application.Notifications;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Tests.Application.Handlers;

public class UpdateTaskItemCommandHandlerTests
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IMediator _mediator;
    private readonly UpdateTaskItemCommandHandler _handler;

    public UpdateTaskItemCommandHandlerTests()
    {
        _taskItemRepository = Substitute.For<ITaskItemRepository>();
        _mediator = Substitute.For<IMediator>();
        _handler = new UpdateTaskItemCommandHandler(_taskItemRepository, _mediator);
    }

    [Fact]
    public async Task Handle_Should_Update_TaskItem_And_Send_Notifications()
    {
        // Arrange
        var task = new TaskItem("Titulo antigo", "Descricao antiga", DateTime.UtcNow.AddDays(2), PriorityEnum.Alta, TaskStatusEnum.Pending, userId: 1);

        _taskItemRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(task);

        var command = new UpdateTaskItemCommand(
            TaskItemId: 1,
            Title: "New Title",
            Description: "New Description",
            DueDate: DateTime.UtcNow.AddDays(5),
            Status: TaskStatusEnum.Completed,
            userId: 1
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _taskItemRepository.Received(1).UpdateAsync(task, Arg.Any<CancellationToken>());

        await _mediator.Received(4).Publish(
            Arg.Any<TaskUpdatedNotification>(),
            Arg.Any<CancellationToken>());

        result.Title.Should().Be("New Title");
        result.Description.Should().Be("New Description");
        result.Status.Should().Be(TaskStatusEnum.Completed);
        result.DueDate.Should().Be(command.DueDate);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_TaskItem_Not_Found()
    {
        // Arrange
        _taskItemRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns((TaskItem?)null);

        var command = new UpdateTaskItemCommand(1, "title", "desc", DateTime.UtcNow, TaskStatusEnum.Pending, 1);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("Não foi possivel encontrar a tarefa.");
    }

    [Fact]
    public async Task Handle_Should_Not_Publish_Notification_When_Fields_Unchanged()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var task = new TaskItem("Same Title", "Same Description", now, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        _taskItemRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(task);

        var command = new UpdateTaskItemCommand(
            TaskItemId: 1,
            Title: "Same Title",
            Description: "Same Description",
            DueDate: now,
            Status: TaskStatusEnum.Pending,
            userId: 1
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _mediator.DidNotReceive().Publish(Arg.Any<TaskUpdatedNotification>(), Arg.Any<CancellationToken>());
        result.Title.Should().Be("Same Title");
    }
}