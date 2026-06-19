using MediatR;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Events;

namespace Velyo.Application.RealTime.EventHandlers;

public class CommentCreatedSignalRHandler : INotificationHandler<CommentCreatedEvent>
{
    private readonly IRealTimeNotifier _realTimeNotifier;

    public CommentCreatedSignalRHandler(IRealTimeNotifier realTimeNotifier)
    {
        _realTimeNotifier = realTimeNotifier;
    }

    public async Task Handle(CommentCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _realTimeNotifier.SendToWorkspaceAsync(
            notification.WorkspaceId,
            "CommentAdded",
            new
            {
                CommentId = notification.Comment.Id,
                TaskId = notification.Comment.TaskId,
                UserId = notification.Comment.UserId,
                Content = notification.Comment.Content,
                CreatedAt = notification.Comment.CreatedAt
            },
            cancellationToken);
    }
}