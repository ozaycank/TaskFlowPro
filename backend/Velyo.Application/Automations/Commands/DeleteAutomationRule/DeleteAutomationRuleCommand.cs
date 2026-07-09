using MediatR;

namespace Velyo.Application.Automations.Commands.DeleteAutomationRule;

public record DeleteAutomationRuleCommand(Guid RuleId) : IRequest;