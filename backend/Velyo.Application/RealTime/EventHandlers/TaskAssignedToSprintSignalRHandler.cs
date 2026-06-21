using MediatR;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Events;

namespace Velyo.Application.RealTime.EventHandlers;

public class TaskAssignedToSprintSignalRHandler : INotificationHandler<TaskAssignedToSprintEvent>
{
    private readonly IRealTimeNotifier _realTimeNotifier;

    public TaskAssignedToSprintSignalRHandler(IRealTimeNotifier realTimeNotifier)
    {
        _realTimeNotifier = realTimeNotifier;
    }

    public async Task Handle(TaskAssignedToSprintEvent notification, CancellationToken cancellationToken)
    {
        await _realTimeNotifier.SendToWorkspaceAsync(
            notification.Task.WorkspaceId,
            "TaskSprintChanged",
            new
            {
                TaskId = notification.Task.Id,
                OldSprintId = notification.OldSprintId,
                NewSprintId = notification.NewSprintId
            },
            cancellationToken);
    }
}