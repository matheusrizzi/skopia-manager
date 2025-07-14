using Mapster;
using MediatR;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Application.Queries;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Queries; 

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;

    public GetAllProjectsQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<List<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Project> projects;

        if(request.UserId.HasValue)
            projects = await _projectRepository.GetAllByUserIdAsync(request.UserId.Value);
        else
            projects = await _projectRepository.GetAllAsync();

        var result = projects.Adapt<List<ProjectDto>>();

        return result;
    }
}