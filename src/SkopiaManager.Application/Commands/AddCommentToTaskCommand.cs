using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace SkopiaManager.Application.Commands;

[ExcludeFromCodeCoverage]
public record AddCommentToTaskCommand(int TaskItemId, string Message, int UserId) : IRequest;

