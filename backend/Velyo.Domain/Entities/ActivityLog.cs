using Velyo.Domain.Common.Models;

namespace Velyo.Domain.Entities;

public class ActivityLog : Entity
{
    public Guid WorkspaceId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public Guid? TaskId { get; private set; }
    public Guid UserId { get; private set; }
    public string EntityType { get; private set; } = null!; // e.g., "Task", "Project", "Document", "Workspace"
    public Guid EntityId { get; private set; }
    public string Action { get; private set; } = null!; // e.g., "Created", "Updated", "StatusChanged", "Deleted"
    public string? Details { get; private set; } // JSON or plain text describing the change

    public DateTimeOffset CreatedAt { get; private set; }

    protected ActivityLog() { }

    private ActivityLog(Guid workspaceId, Guid? projectId, Guid? taskId, Guid userId, string entityType, Guid entityId, string action, string? details)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        ProjectId = projectId;
        TaskId = taskId;
        UserId = userId;
        EntityType = entityType;
        EntityId = entityId;
        Action = action;
        Details = details;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static ActivityLog Create(Guid workspaceId, Guid? projectId, Guid? taskId, Guid userId, string entityType, Guid entityId, string action, string? details = null)
    {
        return new ActivityLog(workspaceId, projectId, taskId, userId, entityType, entityId, action, details);
    }
}