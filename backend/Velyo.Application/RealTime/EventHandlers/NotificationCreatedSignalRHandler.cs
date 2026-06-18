using MediatR;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Events; // Assuming this exists

namespace Velyo.Application.RealTime.EventHandlers;

public class NotificationCreatedSignalRHandler : INotificationHandler<NotificationCreatedEvent>
{
    private readonly IRealTimeNotifier _realTimeNotifier;

    public NotificationCreatedSignalRHandler(IRealTimeNotifier realTimeNotifier)
    {
        _realTimeNotifier = realTimeNotifier;
    }

    public async Task Handle(NotificationCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Hedef kullanıcıya anında yeni bildirimi yolla (React state update için)
        await _realTimeNotifier.SendToUserAsync(
            notification.TargetUserId,
            "ReceiveNotification",
            new
            {
                Id = notification.NotificationId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type.ToString(),
                CreatedAt = DateTimeOffset.UtcNow
            },
            cancellationToken);
    }
}