using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;

namespace SkopiaManager.Tests.Domain;

public class TaskItemTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var title = "Task 1";
        var description = "Description";
        DateTime? dueDate = DateTime.Today.AddDays(5);
        var priority = PriorityEnum.Alta;
        var status = TaskStatusEnum.Pending;
        var userId = 10;

        // Act
        var task = new TaskItem(title, description, dueDate, priority, status, userId);

        // Assert
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(dueDate, task.DueDate);
        Assert.Equal(priority, task.Priority);
        Assert.Equal(status, task.Status);
        Assert.Equal(userId, task.UserId);
        Assert.NotNull(task.Comments);
        Assert.Empty(task.Comments);
        Assert.NotNull(task.ChangeLogs);
        Assert.Empty(task.ChangeLogs);
    }

    [Fact]
    public void UpdateStatus_ShouldChangeStatus_WhenDifferent()
    {
        var task = new TaskItem("Title", "Desc", null, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        task.UpdateStatus(TaskStatusEnum.Completed, 2);

        Assert.Equal(TaskStatusEnum.Completed, task.Status);
    }

    [Fact]
    public void UpdateStatus_ShouldNotChangeStatus_WhenNullOrSame()
    {
        var task = new TaskItem("Title", "Desc", null, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        task.UpdateStatus(null, 2);
        Assert.Equal(TaskStatusEnum.Pending, task.Status);

        task.UpdateStatus(TaskStatusEnum.Pending, 2);
        Assert.Equal(TaskStatusEnum.Pending, task.Status);
    }

    [Fact]
    public void UpdateDescription_ShouldChangeDescription_WhenValid()
    {
        var task = new TaskItem("Title", "Old Desc", null, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        task.UpdateDescription("New Desc", 1);

        Assert.Equal("New Desc", task.Description);
    }

    [Fact]
    public void UpdateDescription_ShouldNotChange_WhenNullOrEmpty()
    {
        var task = new TaskItem("Title", "Old Desc", null, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        task.UpdateDescription(null, 1);
        Assert.Equal("Old Desc", task.Description);

        task.UpdateDescription("", 1);
        Assert.Equal("Old Desc", task.Description);
    }

    [Fact]
    public void UpdateTitle_ShouldChangeTitle_WhenValid()
    {
        var task = new TaskItem("Old Title", "Desc", null, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        task.UpdateTitle("New Title", 1);

        Assert.Equal("New Title", task.Title);
    }

    [Fact]
    public void UpdateTitle_ShouldNotChange_WhenNullOrEmpty()
    {
        var task = new TaskItem("Old Title", "Desc", null, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        task.UpdateTitle(null, 1);
        Assert.Equal("Old Title", task.Title);

        task.UpdateTitle("", 1);
        Assert.Equal("Old Title", task.Title);
    }

    [Fact]
    public void UpdateDueDate_ShouldChangeDueDate_WhenValid()
    {
        var oldDate = DateTime.Today;
        var newDate = oldDate.AddDays(2);
        var task = new TaskItem("Title", "Desc", oldDate, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        task.UpdateDueDate(newDate, 1);

        Assert.Equal(newDate, task.DueDate);
    }

    [Fact]
    public void UpdateDueDate_ShouldNotChange_WhenNull()
    {
        var oldDate = DateTime.Today;
        var task = new TaskItem("Title", "Desc", oldDate, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        task.UpdateDueDate(null, 1);

        Assert.Equal(oldDate, task.DueDate);
    }

    [Fact]
    public void AddComment_ShouldAddCommentToList()
    {
        var task = new TaskItem("Title", "Desc", null, PriorityEnum.Baixa, TaskStatusEnum.Pending, 1);

        task.AddComment("New comment", 5);

        Assert.Single(task.Comments);
        var comment = Assert.Single(task.Comments);
        Assert.Equal("New comment", comment.Message);
        Assert.Equal(5, comment.UserId);
    }
}
