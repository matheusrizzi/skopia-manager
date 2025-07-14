using SkopiaManager.Domain.Enums;

namespace SkopiaManager.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public DateTime? DueDate { get; set; }
    public TaskStatusEnum Status { get; private set; } = TaskStatusEnum.Pending;

    public PriorityEnum Priority { get; private set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public List<Comment> Comments { get; private set; } = new();
    public List<ChangeLog> ChangeLogs { get; private set; } = new();

    public TaskItem()
    {
    }

    public TaskItem(string title, string description, DateTime? dueDate, PriorityEnum priority, TaskStatusEnum status, int userId)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        Status = status;
        UserId = userId;
    }

    public TaskItem UpdateStatus(TaskStatusEnum? newStatus, int user)
    {
        if (!newStatus.HasValue)
            return this;

        if (Status != newStatus)
        {
            var previous = Status;
            Status = newStatus.Value;
        }

        return this;
    }

    public TaskItem UpdateDescription(string newDescription, int user)
    {
        if(string.IsNullOrEmpty(newDescription)) return this;

        if (Description != newDescription)
            Description = newDescription;

        return this;
    }

    public TaskItem UpdateTitle(string newTitle, int user)
    {
        if (string.IsNullOrEmpty(newTitle)) return this;

        if (Title != newTitle)
        {
            Title = newTitle;
        }

        return this;
    }

    public TaskItem UpdateDueDate(DateTime? newDueDate, int user)
    {
        if (!newDueDate.HasValue) return this;

        if (DueDate != newDueDate)
        {
            DueDate = newDueDate;
        }

        return this;
    }

    public void AddComment(string message, int user)
    {
        Comments.Add(new Comment(message, user));
    }
}
