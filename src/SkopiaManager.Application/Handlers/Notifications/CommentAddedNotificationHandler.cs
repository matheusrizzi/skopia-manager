using System.Diagnostics.CodeAnalysis;
using MediatR;
using SkopiaManager.Application.Notifications;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Notifications;

[ExcludeFromCodeCoverage]
public class CommentAddedNotificationHandler : INotificationHandler<CommentAddedNotification>
{
    private readonly IChangeLogRepository _changeLogRepository;

    public CommentAddedNotificationHandler(IChangeLogRepository changeLogRepository)
    {
        _changeLogRepository = changeLogRepository;
    }

    public async Task Handle(CommentAddedNotification notification, CancellationToken cancellationToken)
    {
        var log = new ChangeLog(notification.TaskId, notification.Message, notification.UserId);
        await _changeLogRepository.AddAsync(log, cancellationToken);
    }
}
