using Velyo.Domain.Enums;

namespace Velyo.Application.Automations.Queries.GetAutomationRules;

public record AutomationRuleDto(
    Guid Id,
    Guid WorkspaceId,
    Guid? ProjectId,
    string Name,
    string? Description,
    bool IsActive,
    AutomationTriggerType TriggerType,
    string? TriggerConditionsJson,
    AutomationActionType ActionType,
    string ActionPayloadJson,
    DateTimeOffset CreatedAt);