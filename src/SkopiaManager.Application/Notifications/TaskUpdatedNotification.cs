using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace SkopiaManager.Application.Notifications;

[ExcludeFromCodeCoverage]
public record TaskUpdatedNotification (int TaskId, string Description, int UserId) : INotification
{
}
