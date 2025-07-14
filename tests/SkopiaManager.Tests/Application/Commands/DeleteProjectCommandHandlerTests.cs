using FluentAssertions;
using MediatR;
using NSubstitute;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Handlers.Commands;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Tests.Application.Handlers;

public class DeleteProjectCommandHandlerTests
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectCommandHandlerTests()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _taskItemRepository = Substitute.For<ITaskItemRepository>();
        _handler = new DeleteProjectCommandHandler(_projectRepository, _taskItemRepository);
    }

    [Fact]
    public async Task Handle_Should_Delete_Project_When_Exists_And_Has_No_PendingTasks()
    {
        // Arrange
        var command = new DeleteProjectCommand(1);
        var project = new Project("Projeto 1", 1);

        _projectRepository.GetByIdAsync(command.ProjectId, Arg.Any<CancellationToken>())
            .Returns(project);
        _taskItemRepository.AnyPendingTasksAsync(command.ProjectId, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _projectRepository.Received(1).DeleteAsync(project);
        result.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task Handle_Should_Throw_KeyNotFoundException_When_Project_Does_Not_Exist()
    {
        // Arrange
        var command = new DeleteProjectCommand(99);
        _projectRepository.GetByIdAsync(command.ProjectId, Arg.Any<CancellationToken>())
            .Returns((Project?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("Projeto 99 não encontrado.");
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Project_Has_Pending_Tasks()
    {
        // Arrange
        var command = new DeleteProjectCommand(1);
        var project = new Project("Projeto com pendências", 1);

        _projectRepository.GetByIdAsync(command.ProjectId, Arg.Any<CancellationToken>())
            .Returns(project);
        _taskItemRepository.AnyPendingTasksAsync(command.ProjectId, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Não é possivel excluir um projeto com tarefas pendentes. Por favor complete ou remova as tasks primeiro.");
    }
}