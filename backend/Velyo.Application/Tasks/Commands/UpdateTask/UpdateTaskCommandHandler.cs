using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkspaceAuthorizationService _authorizationService;

    public UpdateTaskCommandHandler(
        ITaskItemRepository taskRepository,
        IUnitOfWork unitOfWork,
        IWorkspaceAuthorizationService authorizationService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null)
        {
            throw new NotFoundException(nameof(TaskItem), request.TaskId);
        }

        // 1. TENANT ISOLATION CHECK: Look up the workspace of the fetched task
        await _authorizationService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        // Domain method encapsulated logic
        task.UpdateDetails(request.Title, request.Description, request.Priority);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}