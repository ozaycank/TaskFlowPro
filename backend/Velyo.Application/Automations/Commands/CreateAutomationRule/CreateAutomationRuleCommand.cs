using MediatR;
using Velyo.Domain.Enums;

namespace Velyo.Application.Automations.Commands.CreateAutomationRule;

public record CreateAutomationRuleCommand(
    Guid WorkspaceId,
    Guid? ProjectId,
    string Name,
    string? Description,
    AutomationTriggerType TriggerType,
    string? TriggerConditionsJson,
    AutomationActionType ActionType,
    string ActionPayloadJson) : IRequest<Guid>;