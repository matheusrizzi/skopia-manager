using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace SkopiaManager.Application.Notifications;

[ExcludeFromCodeCoverage]
public record CommentAddedNotification (int TaskId, string Message, int UserId) : INotification
{
}
