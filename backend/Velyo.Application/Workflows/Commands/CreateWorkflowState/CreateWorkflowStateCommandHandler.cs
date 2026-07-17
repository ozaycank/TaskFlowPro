using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Workflows.Commands.CreateWorkflowState;

public class CreateWorkflowStateCommandHandler : IRequestHandler<CreateWorkflowStateCommand, Guid>
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IWorkspaceAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkflowStateCommandHandler(
        IWorkflowRepository workflowRepository,
        IWorkspaceAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _workflowRepository = workflowRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateWorkflowStateCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _workflowRepository.GetByIdAsync(request.WorkflowId, cancellationToken);

        if (workflow == null)
        {
            throw new NotFoundException(nameof(Workflow), request.WorkflowId.ToString());
        }

        // Only workspace members can modify workflows
        await _authorizationService.AuthorizeMembershipAsync(workflow.WorkspaceId, cancellationToken);

        var newState = workflow.AddState(request.Name, request.Color, request.Category, request.OrderIndex);

        _workflowRepository.Update(workflow);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newState.Id;
    }
}