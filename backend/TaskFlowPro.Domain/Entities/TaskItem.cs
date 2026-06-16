using TaskFlowPro.Domain.Common.Models;
using TaskFlowPro.Domain.Enums;

// Derleyiciye bu dosyadaki TaskStatus'ın bizim enum'ımız olduğunu açıkça söylüyoruz
using TaskStatus = TaskFlowPro.Domain.Enums.TaskStatus;

namespace TaskFlowPro.Domain.Entities;

public class TaskItem : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid ProjectId { get; private set; }

    // FIXED: '= null!;' eklenerek CS8618 derleme hatası kalıcı olarak çözüldü.
    public string Title { get; private set; } = null!;

    public string? Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public PriorityLevel Priority { get; private set; }
    public Guid? AssigneeId { get; private set; }
    public float OrderIndex { get; private set; }

    protected TaskItem() { }

    private TaskItem(Guid workspaceId, Guid projectId, string title, string? description, PriorityLevel priority, float orderIndex)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        ProjectId = projectId;
        Title = title;
        Description = description;
        Status = TaskStatus.Todo;
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
    }

    public void ChangeStatus(TaskStatus newStatus, float newOrderIndex)
    {
        Status = newStatus;
        OrderIndex = newOrderIndex;
    }

    public void UpdateDetails(string title, string? description, PriorityLevel priority)
    {
        Title = title;
        Description = description;
        Priority = priority;
    }
}