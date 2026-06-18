using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Events;

public class NotificationCreatedEvent : DomainEvent
{
    public Guid NotificationId { get; }
    public Guid TargetUserId { get; }
    public string Title { get; }
    public string Message { get; }
    public NotificationType Type { get; }

    public NotificationCreatedEvent(Guid notificationId, Guid targetUserId, string title, string message, NotificationType type)
    {
        NotificationId = notificationId;
        TargetUserId = targetUserId;
        Title = title;
        Message = message;
        Type = type;
    }
}