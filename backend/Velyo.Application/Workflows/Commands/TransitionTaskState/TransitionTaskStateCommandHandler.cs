using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Workflows.Commands.TransitionTaskState;

public record TransitionTaskStateCommand(Guid TaskId, Guid NewStateId, float NewOrderIndex) : IRequest;

public class TransitionTaskStateCommandHandler : IRequestHandler<TransitionTaskStateCommand>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IWorkflowRepository _workflowRepository; // Fetches workflows and states
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public TransitionTaskStateCommandHandler(
        ITaskItemRepository taskRepository,
        IWorkflowRepository workflowRepository,
        IWorkspaceAuthorizationService authService,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _workflowRepository = workflowRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TransitionTaskStateCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);

        // SECURE: Tenant isolation
        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        // Validate that the requested state actually exists and belongs to the workspace
        var stateExists = await _workflowRepository.StateExistsInWorkspaceAsync(task.WorkspaceId, request.NewStateId, cancellationToken);
        if (!stateExists) throw new InvalidOperationException("Invalid target state.");

        // Future Proofing: Here we would check WorkflowTransition rules 
        // (e.g. Can CurrentStateId move to NewStateId based on graph?)

        task.TransitionToState(request.NewStateId, request.NewOrderIndex);

        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}