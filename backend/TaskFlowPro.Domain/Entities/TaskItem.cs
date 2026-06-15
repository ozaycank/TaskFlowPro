using TaskFlowPro.Domain.Common.Models;
using TaskFlowPro.Domain.Enums;

namespace TaskFlowPro.Domain.Entities;

public class TaskItem : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public PriorityLevel Priority { get; private set; }
    public Guid? AssigneeId { get; private set; }
    
    // Crucial for Kanban board Drag & Drop functionality
    public float OrderIndex { get; private set; } 

    protected TaskItem() { }

    private TaskItem(Guid workspaceId, Guid projectId, string title, string? description, PriorityLevel priority, float orderIndex)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        ProjectId = projectId;
        Title = title;
        Description = description;
        Status = TaskStatus.Todo; // Default status
        Priority = priority;
        OrderIndex = orderIndex;
    }

    public static TaskItem Create(Guid workspaceId, Guid projectId, string title, string? description, PriorityLevel priority, float orderIndex)
    {
        return new TaskItem(workspaceId, projectId, title, description, priority, orderIndex);
    }

    public void AssignTo(Guid? assigneeId)
    {
        AssigneeId = assigneeId;
        // Future: this.AddDomainEvent(new TaskAssignedEvent(this));
    }

    public void ChangeStatus(TaskStatus newStatus, float newOrderIndex)
    {
        Status = newStatus;
        OrderIndex = newOrderIndex;
        // Future: this.AddDomainEvent(new TaskStatusChangedEvent(this));
    }

    public void UpdateDetails(string title, string? description, PriorityLevel priority)
    {
        Title = title;
        Description = description;
        Priority = priority;
    }
}