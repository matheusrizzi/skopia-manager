using SkopiaManager.Domain.Enums;

namespace SkopiaManager.Domain.Entities;

public class Project
{
    public Project()
    {
    }
    public Project(string name, int userId)
    {
        Name = name;
        UserId = userId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public List<TaskItem> Tasks { get; private set; } = new();

    public bool CanBeDeleted => !Tasks.Any(t => t.Status == TaskStatusEnum.Pending);

    public bool CanAddTask => Tasks.Count < 20;

    public void AddTask(TaskItem task)
    {
        if (!CanAddTask)
            throw new InvalidOperationException("Este projeto já atingiu o número máximo (20) de tasks.");

        Tasks.Add(task);
    }
}
