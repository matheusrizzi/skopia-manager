using MediatR;
using SkopiaManager.Application.Commands;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IProjectRepository _projectRepository;

    public CreateProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(request.Name, request.UserId);
        var Tasks = request.Tasks.Select(t => new TaskItem(t.Title, t.Description, t.DueDate, t.Priority, TaskStatusEnum.Pending, request.UserId)).ToList();

        foreach (var task in Tasks)
            project.AddTask(task);

        await _projectRepository.AddAsync(project, cancellationToken);

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            // Aqui você pode retornar as tarefas também se quiser
        };
    }
}
