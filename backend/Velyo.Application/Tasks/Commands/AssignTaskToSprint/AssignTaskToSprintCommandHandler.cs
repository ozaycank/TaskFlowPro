using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Tasks.Commands.AssignTaskToSprint;

public class AssignTaskToSprintCommandHandler : IRequestHandler<AssignTaskToSprintCommand>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public AssignTaskToSprintCommandHandler(ITaskItemRepository taskRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AssignTaskToSprintCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);

        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        task.AssignToSprint(request.SprintId);

        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}