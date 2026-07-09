using MediatR;

namespace Velyo.Application.Automations.Commands.ToggleAutomationRule;

public record ToggleAutomationRuleCommand(Guid RuleId, bool IsActive) : IRequest;