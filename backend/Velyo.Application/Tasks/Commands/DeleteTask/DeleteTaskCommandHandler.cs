using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IWorkspaceAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskCommandHandler(
        ITaskItemRepository taskItemRepository,
        IWorkspaceAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _taskItemRepository = taskItemRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskItemRepository.GetByIdAsync(request.TaskId, cancellationToken);

        if (task == null)
        {
            throw new NotFoundException(nameof(TaskItem), request.TaskId.ToString());
        }

        // Ensure user has access to the workspace this task belongs to
        await _authorizationService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        _taskItemRepository.Delete(task);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}