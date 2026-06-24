using MediatR;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Events;

namespace Velyo.Application.RealTime.EventHandlers;

public class TaskStateTransitionedSignalRHandler : INotificationHandler<TaskStateTransitionedEvent>
{
    private readonly IRealTimeNotifier _realTimeNotifier;

    public TaskStateTransitionedSignalRHandler(IRealTimeNotifier realTimeNotifier)
    {
        _realTimeNotifier = realTimeNotifier;
    }

    public async Task Handle(TaskStateTransitionedEvent notification, CancellationToken cancellationToken)
    {
        await _realTimeNotifier.SendToWorkspaceAsync(
            notification.Task.WorkspaceId,
            "TaskMoved",
            new
            {
                TaskId = notification.Task.Id,
                ProjectId = notification.Task.ProjectId,
                NewStateId = notification.NewStateId,
                NewOrderIndex = notification.Task.OrderIndex
            },
            cancellationToken);
    }
}