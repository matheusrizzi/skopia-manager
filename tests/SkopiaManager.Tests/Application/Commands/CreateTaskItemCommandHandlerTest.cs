using FluentAssertions;
using NSubstitute;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Handlers.Commands;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Tests.Application.Handlers;

public class CreateTaskItemCommandHandlerTest
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly CreateTaskItemCommandHandler _handler;

    public CreateTaskItemCommandHandlerTest()
    {
        _taskItemRepository = Substitute.For<ITaskItemRepository>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _handler = new CreateTaskItemCommandHandler(_taskItemRepository, _projectRepository);
    }

    [Fact]
    public async Task Handle_Should_Create_TaskItem_And_Return_Dto_When_ProjectExists_And_HasSpace()
    {
        // Arrange
        var project = new Project("Projeto Teste", 1); 
        var command = new CreateTaskItemCommand("Título","Descrição",DateTime.UtcNow.AddDays(1),1, PriorityEnum.Alta,42);

        _projectRepository.GetByIdAsync(command.ProjectId).Returns(project);

        TaskItem savedTask = null!;
        await _taskItemRepository.AddAsync(Arg.Do<TaskItem>(t => savedTask = t));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _taskItemRepository.Received(1).AddAsync(Arg.Any<TaskItem>());

        result.Should().NotBeNull();
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.Priority.Should().Be(command.Priority);
        result.Status.Should().Be(TaskStatusEnum.Pending);

        savedTask.Should().NotBeNull();
        savedTask.Project.Should().Be(project);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_ProjectNotFound()
    {
        // Arrange
        var command = new CreateTaskItemCommand("Título","Descrição",DateTime.UtcNow.AddDays(1),1,PriorityEnum.Alta,99);

        _projectRepository.GetByIdAsync(command.ProjectId).Returns((Project?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Projeto não encontrado.");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_Project_Has_TooMany_Tasks()
    {
        // Arrange
        var project = new Project("Lotado", 1);

        for (int i = 0; i < 20; i++)
            project.AddTask(new TaskItem("T", "D", DateTime.UtcNow, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1));

        var command = new CreateTaskItemCommand("Nova Tarefa","Nova Descrição",DateTime.UtcNow.AddDays(1),44,PriorityEnum.Alta,42);

        _projectRepository.GetByIdAsync(command.ProjectId).Returns(project);

        //Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Limite de tarefas por projeto atingido.");
    }
}