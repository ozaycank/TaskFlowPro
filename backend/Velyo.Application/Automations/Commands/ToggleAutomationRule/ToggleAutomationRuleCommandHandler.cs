using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Automations.Commands.ToggleAutomationRule;

public class ToggleAutomationRuleCommandHandler : IRequestHandler<ToggleAutomationRuleCommand>
{
    private readonly IAutomationRuleRepository _repository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleAutomationRuleCommandHandler(IAutomationRuleRepository repository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ToggleAutomationRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = await _repository.GetByIdAsync(request.RuleId, cancellationToken);
        if (rule == null) throw new NotFoundException(nameof(AutomationRule), request.RuleId);

        await _authService.AuthorizeRoleAsync(rule.WorkspaceId, Domain.Enums.WorkspaceRole.Admin, cancellationToken);

        rule.ToggleActive(request.IsActive);

        _repository.Update(rule);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}