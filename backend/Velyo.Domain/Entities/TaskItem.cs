using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

// Derleyiciye bu dosyadaki TaskStatus'Ä±n bizim enum'Ä±mÄ±z olduÄŸunu aÃ§Ä±kÃ§a sÃ¶ylÃ¼yoruz
using TaskStatus = Velyo.Domain.Enums.TaskStatus;

namespace Velyo.Domain.Entities;

public class TaskItem : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid ProjectId { get; private set; }
    public Dictionary<string, string> CustomFieldsData { get; private set; } = new();
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
    // Modifying the domain method to handle dynamic fields safely
    public void UpdateCustomField(Guid fieldDefinitionId, string value)
    {
        CustomFieldsData[fieldDefinitionId.ToString()] = value;
        // AddDomainEvent(new TaskUpdatedEvent(this, ...)) should be triggered
    }

    public void RemoveCustomField(Guid fieldDefinitionId)
    {
        if (CustomFieldsData.ContainsKey(fieldDefinitionId.ToString()))
        {
            CustomFieldsData.Remove(fieldDefinitionId.ToString());
        }
    }
}