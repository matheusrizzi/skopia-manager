using System.Diagnostics.CodeAnalysis;

namespace SkopiaManager.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Comment
{
    public int Id { get; set; }
    public string Message { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public int TaskItemId { get; set; }

    public Comment()
    {
    }

    public Comment(string message, int user)
    {
        Message = message;
        UserId = user;
        CreatedAt = DateTime.UtcNow;
    }
}
