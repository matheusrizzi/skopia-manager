using FluentAssertions;
using NSubstitute;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Handlers.Queries;
using SkopiaManager.Application.Queries;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Tests.Application.Queries;

public class GetTasksByProjectQueryHandlerTests
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly GetTasksByProjectQueryHandler _handler;

    public GetTasksByProjectQueryHandlerTests()
    {
        _taskItemRepository = Substitute.For<ITaskItemRepository>();
        _handler = new GetTasksByProjectQueryHandler(_taskItemRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_List_Of_TaskItemDto_When_Tasks_Found()
    {
        // Arrange
        int projectId = 10;
        var query = new GetTasksByProjectQuery(projectId);

        var taskList = new List<TaskItem>
            {
                new TaskItem("Título 1", "Desc 1", DateTime.UtcNow.AddDays(1), PriorityEnum.Alta, TaskStatusEnum.Pending, 1),
                new TaskItem("Título 2", "Desc 2", DateTime.UtcNow.AddDays(2), PriorityEnum.Baixa, TaskStatusEnum.Completed, 1)
            };

        _taskItemRepository.GetTasksByProjectIdAsync(projectId)
            .Returns(taskList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().AllBeOfType<TaskItemDto>();
        result[0].Title.Should().Be("Título 1");
        result[1].Title.Should().Be("Título 2");

        await _taskItemRepository.Received(1).GetTasksByProjectIdAsync(projectId);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Tasks_Found()
    {
        // Arrange
        int projectId = 99;
        var query = new GetTasksByProjectQuery(projectId);

        _taskItemRepository.GetTasksByProjectIdAsync(projectId)
            .Returns(new List<TaskItem>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        await _taskItemRepository.Received(1).GetTasksByProjectIdAsync(projectId);
    }
}