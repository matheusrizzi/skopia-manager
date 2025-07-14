using FluentAssertions;
using MediatR;
using NSubstitute;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Handlers.Commands;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Tests.Application.Handlers;

public class DeleteTaskItemCommandHandlerTests
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly DeleteTaskItemCommandHandler _handler;

    public DeleteTaskItemCommandHandlerTests()
    {
        _taskItemRepository = Substitute.For<ITaskItemRepository>();
        _handler = new DeleteTaskItemCommandHandler(_taskItemRepository);
    }

    [Fact]
    public async Task Handle_Should_Delete_TaskItem_When_It_Exists()
    {
        // Arrange
        var command = new DeleteTaskItemCommand(1);
        var taskItem = new TaskItem("Título", "Descrição", DateTime.UtcNow.AddDays(1), PriorityEnum.Alta, TaskStatusEnum.Pending, userId: 42);

        _taskItemRepository.GetByIdAsync(command.TaskItemId, Arg.Any<CancellationToken>())
            .Returns(taskItem);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _taskItemRepository.Received(1).DeleteAsync(taskItem, Arg.Any<CancellationToken>());
        result.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task Handle_Should_Throw_KeyNotFoundException_When_TaskItem_Not_Found()
    {
        // Arrange
        var command = new DeleteTaskItemCommand(999);

        _taskItemRepository.GetByIdAsync(command.TaskItemId, Arg.Any<CancellationToken>())
            .Returns((TaskItem?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("Tarefa não encontrada na base de dados.");
    }
}