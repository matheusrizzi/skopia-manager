using MediatR;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Commands;

public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, TaskItemDto>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IProjectRepository _projectRepository;

    public CreateTaskItemCommandHandler(
        ITaskItemRepository taskItemRepository,
        IProjectRepository projectRepository)
    {
        _taskItemRepository = taskItemRepository;
        _projectRepository = projectRepository;
    }

    public async Task<TaskItemDto> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
            throw new Exception("Projeto não encontrado.");

        if (!project.CanAddTask)
            throw new Exception("Limite de tarefas por projeto atingido.");

        var taskItem = new TaskItem(request.Title, request.Description, request.DueDate, request.Priority, TaskStatusEnum.Pending, request.UserId)
        {
            Project = project,
        };

        await _taskItemRepository.AddAsync(taskItem);

        return new TaskItemDto
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            DueDate = taskItem.DueDate,
            Status = taskItem.Status,
            Priority = taskItem.Priority,
            ProjectId = taskItem.ProjectId
        };
    }
}
