using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Workflows.Commands.UpdateWorkflowState;

public class UpdateWorkflowStateCommandHandler : IRequestHandler<UpdateWorkflowStateCommand>
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IWorkspaceAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWorkflowStateCommandHandler(
        IWorkflowRepository workflowRepository,
        IWorkspaceAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _workflowRepository = workflowRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateWorkflowStateCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _workflowRepository.GetByIdAsync(request.WorkflowId, cancellationToken);

        if (workflow == null)
        {
            throw new NotFoundException(nameof(Workflow), request.WorkflowId.ToString());
        }

        await _authorizationService.AuthorizeMembershipAsync(workflow.WorkspaceId, cancellationToken);

        workflow.UpdateState(request.StateId, request.Name, request.Color, request.Category, request.OrderIndex);

        _workflowRepository.Update(workflow);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}