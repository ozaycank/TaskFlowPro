using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Automations.Commands.CreateAutomationRule;

public class CreateAutomationRuleCommandHandler : IRequestHandler<CreateAutomationRuleCommand, Guid>
{
    private readonly IAutomationRuleRepository _repository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAutomationRuleCommandHandler(IAutomationRuleRepository repository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateAutomationRuleCommand request, CancellationToken cancellationToken)
    {
        // Require Admin privileges to create automations
        await _authService.AuthorizeRoleAsync(request.WorkspaceId, Domain.Enums.WorkspaceRole.Admin, cancellationToken);

        var rule = AutomationRule.Create(
            request.WorkspaceId, request.ProjectId, request.Name, request.Description,
            request.TriggerType, request.TriggerConditionsJson, request.ActionType, request.ActionPayloadJson);

        _repository.Add(rule);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return rule.Id;
    }
}