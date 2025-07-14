using FluentAssertions;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;

namespace SkopiaManager.Tests.Domain;

public class ProjectTests
{
    [Fact]
    public void Constructor_WithoutParameters_ShouldInitializeTasksList()
    {
        var project = new Project();

        project.Tasks.Should().NotBeNull();
        project.Tasks.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithParameters_ShouldSetNameAndUserId()
    {
        var project = new Project("Projeto X", 123);

        project.Name.Should().Be("Projeto X");
        project.UserId.Should().Be(123);
    }

    [Fact]
    public void CanBeDeleted_ShouldBeFalse_IfThereArePendingTasks()
    {
        var project = new Project();
        project.Tasks.Add(new TaskItem().UpdateStatus(TaskStatusEnum.Pending, 1));
            
        project.CanBeDeleted.Should().BeFalse();
    }

    [Fact]
    public void CanBeDeleted_ShouldBeTrue_IfNoPendingTasks()
    {
        var project = new Project();
        project.Tasks.Add(new TaskItem().UpdateStatus(TaskStatusEnum.Completed, 1));
        project.Tasks.Add(new TaskItem().UpdateStatus(TaskStatusEnum.Completed, 1));

        project.CanBeDeleted.Should().BeTrue();
    }

    [Fact]
    public void CanAddTask_ShouldBeTrue_IfTasksCountIsLessThan20()
    {
        var project = new Project();
        for (int i = 0; i < 19; i++)
            project.Tasks.Add(new TaskItem());

        project.CanAddTask.Should().BeTrue();
    }

    [Fact]
    public void CanAddTask_ShouldBeFalse_IfTasksCountIs20OrMore()
    {
        var project = new Project();
        for (int i = 0; i < 20; i++)
            project.Tasks.Add(new TaskItem());

        project.CanAddTask.Should().BeFalse();
    }

    [Fact]
    public void AddTask_ShouldAddTask_WhenUnderLimit()
    {
        var project = new Project();
        var task = new TaskItem();

        project.AddTask(task);

        project.Tasks.Should().Contain(task);
    }

    [Fact]
    public void AddTask_ShouldThrowException_WhenAtTaskLimit()
    {
        var project = new Project();
        for (int i = 0; i < 20; i++)
            project.Tasks.Add(new TaskItem());

        var newTask = new TaskItem();

        Action act = () => project.AddTask(newTask);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Este projeto já atingiu o número máximo (20) de tasks.");
    }
}
