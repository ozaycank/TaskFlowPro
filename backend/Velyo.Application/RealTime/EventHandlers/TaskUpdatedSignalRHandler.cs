using MediatR;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Events;

namespace Velyo.Application.RealTime.EventHandlers;

public class TaskUpdatedSignalRHandler : INotificationHandler<TaskUpdatedEvent>
{
    private readonly IRealTimeNotifier _realTimeNotifier;

    public TaskUpdatedSignalRHandler(IRealTimeNotifier realTimeNotifier)
    {
        _realTimeNotifier = realTimeNotifier;
    }

    public async Task Handle(TaskUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Sadece bu görevin ait olduğu Workspace odasına yayınla! Tenant Isolation.
        await _realTimeNotifier.SendToWorkspaceAsync(
            notification.Task.WorkspaceId,
            "TaskUpdated",
            new
            {
                TaskId = notification.Task.Id,
                NewStatus = notification.Task.Status.ToString(),
                Title = notification.Task.Title
            },
            cancellationToken);
    }
}