using SkopiaManager.Domain.Enums;

namespace SkopiaManager.Application.Dtos;

public class TaskItemDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public TaskStatusEnum Status { get; set; }

    public PriorityEnum Priority { get; set; }

    public int ProjectId { get; set; }

    //public List<CommentDto>? Comments { get; set; } // opcional
}
