using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Entities;

public class Notification : Entity
{
    public Guid WorkspaceId { get; private set; }
    public Guid UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Title { get; private set; } = null!;
    public string Message { get; private set; } = null!;
    public string? ActionUrl { get; private set; }
    public bool IsRead { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    protected Notification() { }

    private Notification(Guid workspaceId, Guid userId, NotificationType type, string title, string message, string? actionUrl)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        UserId = userId;
        Type = type;
        Title = title;
        Message = message;
        ActionUrl = actionUrl;
        IsRead = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Notification Create(Guid workspaceId, Guid userId, NotificationType type, string title, string message, string? actionUrl = null)
    {
        return new Notification(workspaceId, userId, type, title, message, actionUrl);
    }

    public void MarkAsRead()
    {
        if (!IsRead) IsRead = true;
    }
}