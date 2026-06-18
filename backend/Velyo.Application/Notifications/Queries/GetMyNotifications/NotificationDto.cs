using Velyo.Domain.Enums;
namespace Velyo.Application.Notifications.Queries.GetMyNotifications;

public record NotificationDto(Guid Id, Guid WorkspaceId, NotificationType Type, string Title, string Message, string? ActionUrl, bool IsRead, DateTimeOffset CreatedAt);