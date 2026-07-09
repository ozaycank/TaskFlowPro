using MediatR;

namespace Velyo.Application.Automations.Queries.GetAutomationRules;

public record GetAutomationRulesQuery(Guid WorkspaceId, Guid? ProjectId) : IRequest<IEnumerable<AutomationRuleDto>>;