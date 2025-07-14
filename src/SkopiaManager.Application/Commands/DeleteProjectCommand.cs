using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace SkopiaManager.Application.Commands;

[ExcludeFromCodeCoverage]
public record DeleteProjectCommand(int ProjectId) : IRequest
{
}