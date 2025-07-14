using MediatR;
using SkopiaManager.Application.Dtos;

namespace SkopiaManager.Application.Queries;

public record GetAllProjectsQuery(int? UserId) : IRequest<List<ProjectDto>>
{
}
