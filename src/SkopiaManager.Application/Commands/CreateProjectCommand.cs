using System.Diagnostics.CodeAnalysis;
using MediatR;
using SkopiaManager.Application.Dtos;

namespace SkopiaManager.Application.Commands;

[ExcludeFromCodeCoverage]
public record CreateProjectCommand(string Name, int UserId, List<CreateTaskDto> Tasks) : IRequest<ProjectDto>;
