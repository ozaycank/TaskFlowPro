using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Entities;

public class AutomationRule : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid? ProjectId { get; private set; } // If null, applies to all projects in workspace
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    // Trigger
    public AutomationTriggerType TriggerType { get; private set; }
    public string? TriggerConditionsJson { get; private set; } // JSON array of conditions

    // Action
    public AutomationActionType ActionType { get; private set; }
    public string ActionPayloadJson { get; private set; } = null!; // JSON object required for action execution

    protected AutomationRule() { }

    private AutomationRule(
        Guid workspaceId,
        Guid? projectId,
        string name,
        string? description,
        AutomationTriggerType triggerType,
        string? triggerConditionsJson,
        AutomationActionType actionType,
        string actionPayloadJson)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        ProjectId = projectId;
        Name = name;
        Description = description;
        IsActive = true;
        TriggerType = triggerType;
        TriggerConditionsJson = triggerConditionsJson;
        ActionType = actionType;
        ActionPayloadJson = actionPayloadJson;
    }

    public static AutomationRule Create(
        Guid workspaceId, Guid? projectId, string name, string? description,
        AutomationTriggerType triggerType, string? triggerConditionsJson,
        AutomationActionType actionType, string actionPayloadJson)
    {
        return new AutomationRule(workspaceId, projectId, name, description, triggerType, triggerConditionsJson, actionType, actionPayloadJson);
    }

    public void Update(string name, string? description, AutomationTriggerType triggerType, string? triggerConditionsJson, AutomationActionType actionType, string actionPayloadJson)
    {
        Name = name;
        Description = description;
        TriggerType = triggerType;
        TriggerConditionsJson = triggerConditionsJson;
        ActionType = actionType;
        ActionPayloadJson = actionPayloadJson;
    }

    public void ToggleActive(bool isActive)
    {
        IsActive = isActive;
    }
}