using FluentAssertions;
using NSubstitute;
using SkopiaManager.Application.Handlers.Queries;
using SkopiaManager.Application.Queries;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Tests.Application.Queries;

public class GetAllProjectsQueryHandlerTests
{
    private readonly IProjectRepository _projectRepository;
    private readonly GetAllProjectsQueryHandler _handler;

    public GetAllProjectsQueryHandlerTests()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _handler = new GetAllProjectsQueryHandler(_projectRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Projects_When_UserId_Not_Provided()
    {
        // Arrange
        var projects = new List<Project>
            {
                new Project("Projeto A", 1),
                new Project("Projeto B", 2)
            };

        _projectRepository.GetAllAsync().Returns(projects);

        var query = new GetAllProjectsQuery(null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Select(p => p.Name).Should().Contain(new[] { "Projeto A", "Projeto B" });
        await _projectRepository.Received(1).GetAllAsync();
        await _projectRepository.DidNotReceive().GetAllByUserIdAsync(Arg.Any<int>());
    }

    [Fact]
    public async Task Handle_Should_Return_Projects_For_Specific_User()
    {
        // Arrange
        var projects = new List<Project>
            {
                new Project("Projeto do Usuário", 42)
            };

        _projectRepository.GetAllByUserIdAsync(42).Returns(projects);

        var query = new GetAllProjectsQuery(42);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Projeto do Usuário");
        await _projectRepository.Received(1).GetAllByUserIdAsync(42);
        await _projectRepository.DidNotReceive().GetAllAsync();
    }

    [Fact]
    public async Task Handle_Should_Return_EmptyList_When_No_Projects_Found()
    {
        // Arrange
        _projectRepository.GetAllAsync().Returns(Enumerable.Empty<Project>());
        var query = new GetAllProjectsQuery(null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
