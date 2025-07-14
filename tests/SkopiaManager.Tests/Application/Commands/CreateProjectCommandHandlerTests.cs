using NSubstitute;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Handlers.Commands;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Tests.Application.Handlers;

public class CreateProjectCommandHandlerTests
{
    private readonly IProjectRepository _projectRepository;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _handler = new CreateProjectCommandHandler(_projectRepository);
    }

    [Fact]
    public async Task Handle_Should_Create_Project_With_Tasks_And_Return_Dto()
    {
        // Arrange

        var myTasks = new List<CreateTaskDto>
                {
                    new CreateTaskDto(){ Title = "Tarefa 1", Description="Minha tarefa 1", DueDate = DateTime.UtcNow.AddDays(1)},
                    new CreateTaskDto(){ Title = "Tarefa 2", Description="Minha tarefa 2", DueDate = DateTime.UtcNow.AddDays(1)}
                };

        var command = new CreateProjectCommand("Projeto 1", 1, myTasks);

        Project savedProject = null!;

        await _projectRepository.AddAsync(Arg.Do<Project>(p => savedProject = p), Arg.Any<CancellationToken>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _projectRepository.Received(1).AddAsync(Arg.Any<Project>(), Arg.Any<CancellationToken>());

        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(0, result.Id); 

        Assert.NotNull(savedProject);
        Assert.Equal(command.Name, savedProject.Name);
        Assert.Equal(command.UserId, savedProject.UserId);
        Assert.Equal(2, savedProject.Tasks.Count);
    }

    [Fact]
    public async Task Handle_Should_Create_Project_Without_Tasks_And_Return_Dto()
    {
        // Arrange
        var emptyTasks = new List<CreateTaskDto>(); 

        var command = new CreateProjectCommand("Projeto Sem Tarefas", 1, emptyTasks);

        Project savedProject = null!;

        await _projectRepository.AddAsync(Arg.Do<Project>(p => savedProject = p), Arg.Any<CancellationToken>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _projectRepository.Received(1).AddAsync(Arg.Any<Project>(), Arg.Any<CancellationToken>());

        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(0, result.Id); // ID default

        Assert.NotNull(savedProject);
        Assert.Equal(command.Name, savedProject.Name);
        Assert.Equal(command.UserId, savedProject.UserId);
        Assert.Empty(savedProject.Tasks); // Nenhuma tarefa adicionada
    }

}
