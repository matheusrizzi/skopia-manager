using System.Diagnostics.CodeAnalysis;
using MediatR;
using SkopiaManager.Application.Notifications;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;

namespace SkopiaManager.Application.Handlers.Notifications;

[ExcludeFromCodeCoverage]
public class TaskUpdatedNotificationHandler : INotificationHandler<TaskUpdatedNotification>
{
    private readonly IChangeLogRepository _changeLogRepository;

    public TaskUpdatedNotificationHandler(IChangeLogRepository changeLogRepository)
    {
        _changeLogRepository = changeLogRepository;
    }

    public async Task Handle(TaskUpdatedNotification notification, CancellationToken cancellationToken)
    {
        var log = new ChangeLog(notification.TaskId, notification.Description, notification.UserId);

        await _changeLogRepository.AddAsync(log, cancellationToken);
    }
}
