using System.Diagnostics.CodeAnalysis;

namespace SkopiaManager.Domain.Entities;

[ExcludeFromCodeCoverage]
public class ChangeLog
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public DateTime ModifiedAt { get; set; }

    public int TaskItemId { get; set; }

    public ChangeLog(int taskItemId, string description, int userId)
    {
        TaskItemId = taskItemId;
        Description = description;
        UserId = userId;
        ModifiedAt = DateTime.UtcNow;
    }
}
