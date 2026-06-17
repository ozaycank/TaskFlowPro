using Velyo.Domain.Common.Models;

namespace Velyo.Domain.Entities;

public class ActivityLog : Entity
{
    public Guid WorkspaceId { get; private set; }
    public Guid UserId { get; private set; }
    public string Action { get; private set; } = null!;
    public string Details { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }

    protected ActivityLog() { }

    private ActivityLog(Guid workspaceId, Guid userId, string action, string details)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        UserId = userId;
        Action = action;
        Details = details;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static ActivityLog Create(Guid workspaceId, Guid userId, string action, string details)
    {
        return new ActivityLog(workspaceId, userId, action, details);
    }
}