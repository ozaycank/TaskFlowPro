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
    public Guid? SprintId { get; private set; }
    public PriorityLevel Priority { get; private set; }
    public Guid? AssigneeId { get; private set; }
    public float OrderIndex { get; private set; }
    public Dictionary<string, string> CustomFieldsData { get; private set; } = new();
    public DateTimeOffset? DueDate { get; private set; }
    public Guid? ParentTaskId { get; private set; } // Epic veya Ana Görev bağlantısı
    public List<string> Labels { get; private set; } = new(); // Etiketler (Tags)

    protected TaskItem() { }

    private TaskItem(Guid workspaceId, Guid projectId, string title, string? description, PriorityLevel priority, Guid stateId, float orderIndex, DateTimeOffset? dueDate, Guid? parentTaskId = null)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        ProjectId = projectId;
        Title = title;
        Description = description;
        StateId = stateId;
        Priority = priority;
        OrderIndex = orderIndex;
        DueDate = dueDate;
        ParentTaskId = parentTaskId; // YENİ
    }

    public void AssignToSprint(Guid? sprintId)
    {
        var oldSprintId = SprintId;
        SprintId = sprintId;
        AddDomainEvent(new TaskAssignedToSprintEvent(this, oldSprintId, sprintId));
    }

    public static TaskItem Create(Guid workspaceId, Guid projectId, string title, string? description, PriorityLevel priority, Guid stateId, float orderIndex, DateTimeOffset? dueDate, Guid? parentTaskId = null)
    {
        return new TaskItem(workspaceId, projectId, title, description, priority, stateId, orderIndex, dueDate, parentTaskId);
    }

    public void AssignTo(Guid? assigneeId)
    {
        AssigneeId = assigneeId;
    }

    public void TransitionToState(Guid newStateId, float newOrderIndex)
    {
        StateId = newStateId;
        OrderIndex = newOrderIndex;
        AddDomainEvent(new TaskStateTransitionedEvent(this, newStateId));
    }

    public void UpdateDetails(string title, string? description, PriorityLevel priority, DateTimeOffset? dueDate)
    {
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
    }

    // --- YENİ: Etiket Yönetimi ---
    public void AddLabel(string label)
    {
        if (!string.IsNullOrWhiteSpace(label) && !Labels.Contains(label))
        {
            Labels.Add(label);
        }
    }

    public void RemoveLabel(string label)
    {
        Labels.Remove(label);
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