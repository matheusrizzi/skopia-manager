using SkopiaManager.Domain.Enums;

namespace SkopiaManager.Application.Dtos;

public class CreateTaskDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public PriorityEnum Priority { get; set; } = PriorityEnum.Media;
}