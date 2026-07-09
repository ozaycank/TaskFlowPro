using MediatR;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Application.Automations.Queries.GetAutomationRules;

public class GetAutomationRulesQueryHandler : IRequestHandler<GetAutomationRulesQuery, IEnumerable<AutomationRuleDto>>
{
    private readonly IAutomationRuleRepository _repository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetAutomationRulesQueryHandler(IAutomationRuleRepository repository, IWorkspaceAuthorizationService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<IEnumerable<AutomationRuleDto>> Handle(GetAutomationRulesQuery request, CancellationToken cancellationToken)
    {
        await _authService.AuthorizeMembershipAsync(request.WorkspaceId, cancellationToken);

        var rules = request.ProjectId.HasValue
            ? await _repository.GetByProjectIdAsync(request.ProjectId.Value, cancellationToken)
            : await _repository.GetByWorkspaceIdAsync(request.WorkspaceId, cancellationToken);

        return rules.Select(r => new AutomationRuleDto(
            r.Id, r.WorkspaceId, r.ProjectId, r.Name, r.Description, r.IsActive,
            r.TriggerType, r.TriggerConditionsJson, r.ActionType, r.ActionPayloadJson, r.CreatedAt))
            .OrderByDescending(r => r.CreatedAt);
    }
}