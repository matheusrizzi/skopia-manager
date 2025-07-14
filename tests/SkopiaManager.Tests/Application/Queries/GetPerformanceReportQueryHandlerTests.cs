using FluentAssertions;
using NSubstitute;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Handlers.Queries;
using SkopiaManager.Application.Interfaces;
using SkopiaManager.Application.Queries;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Tests.Application.Queries;

public class GetPerformanceReportQueryHandlerTests
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;
    private readonly GetPerformanceReportQueryHandler _handler;

    public GetPerformanceReportQueryHandlerTests()
    {
        _reportRepository = Substitute.For<IReportRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new GetPerformanceReportQueryHandler(_reportRepository, _userRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_Report_When_User_Is_Manager()
    {
        // Arrange
        var userId = 42;
        var user = new User { Id = userId, Role = "Gerente" };
        var expectedReport = new List<PerformanceReportDto>
            {
                new PerformanceReportDto(userId, "Usuario 999",5)
            };

        _userRepository.GetByIdAsync(userId).Returns(user);
        _reportRepository.GetPerformanceReportAsync(Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(expectedReport);

        var query = new GetPerformanceReportQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedReport);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
    {
        // Arrange
        var query = new GetPerformanceReportQuery(99);
        _userRepository.GetByIdAsync(99).Returns((User)null);

        // Act
        Func<Task> act = () => _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Usuário não encontrado.");
    }

    [Fact]
    public async Task Handle_Should_Throw_UnauthorizedAccess_When_User_Is_Not_Manager()
    {
        // Arrange
        var userId = 77;
        var user = new User { Id = userId, Role = "Colaborador" };

        _userRepository.GetByIdAsync(userId).Returns(user);
        var query = new GetPerformanceReportQuery(userId);

        // Act
        Func<Task> act = () => _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Somente usuários com a função de gerente podem acessar este relatório.");
    }
}