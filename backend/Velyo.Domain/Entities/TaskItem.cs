using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;
using Velyo.Domain.Events;

namespace Velyo.Domain.Entities;

public class TaskItem : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid StateId { get; private set; }

    public PriorityLevel Priority { get; private set; }
    public Guid? AssigneeId { get; private set; }
    public float OrderIndex { get; private set; }

    // Phase 20'den gelen JSONB verisi
    public Dictionary<string, string> CustomFieldsData { get; private set; } = new();

    protected TaskItem() { }

    private TaskItem(Guid workspaceId, Guid projectId, string title, string? description, PriorityLevel priority, Guid stateId, float orderIndex)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        ProjectId = projectId;
        Title = title;
        Description = description;
        StateId = stateId; // Artık Task oluşturulurken bir Başlangıç State'i (Örn: Todo Guid'i) verilmeli
        Priority = priority;
        OrderIndex = orderIndex;
    }

    public static TaskItem Create(Guid workspaceId, Guid projectId, string title, string? description, PriorityLevel priority, Guid stateId, float orderIndex)
    {
        return new TaskItem(workspaceId, projectId, title, description, priority, stateId, orderIndex);
    }

    public void AssignTo(Guid? assigneeId)
    {
        AssigneeId = assigneeId;
    }

    public void TransitionToState(Guid newStateId, float newOrderIndex)
    {
        StateId = newStateId;
        OrderIndex = newOrderIndex;

        // Fire Domain Event for Real-Time UI updates and Activity Logging
        AddDomainEvent(new TaskStateTransitionedEvent(this, newStateId));
    }

    public void UpdateDetails(string title, string? description, PriorityLevel priority)
    {
        Title = title;
        Description = description;
        Priority = priority;
    }

    public void UpdateCustomField(Guid fieldDefinitionId, string value)
    {
        CustomFieldsData[fieldDefinitionId.ToString()] = value;
    }

    public void RemoveCustomField(Guid fieldDefinitionId)
    {
        if (CustomFieldsData.ContainsKey(fieldDefinitionId.ToString()))
        {
            CustomFieldsData.Remove(fieldDefinitionId.ToString());
        }
    }
}