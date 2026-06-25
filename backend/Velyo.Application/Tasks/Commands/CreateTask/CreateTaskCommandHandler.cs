using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkspaceAuthorizationService _authorizationService;

    public CreateTaskCommandHandler(
        ITaskItemRepository taskRepository,
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IWorkspaceAuthorizationService authorizationService)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        // 1. TENANT ISOLATION CHECK: Fail fast before processing any logic
        await _authorizationService.AuthorizeMembershipAsync(request.WorkspaceId, cancellationToken);

        // Business Rule: Validate Project existence and ownership
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null || project.WorkspaceId != request.WorkspaceId)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        var task = TaskItem.Create(
            request.WorkspaceId,
            request.ProjectId,
            request.Title,
            request.Description,
            request.Priority,
            request.StateId,
            request.OrderIndex,
            request.DueDate);

        _taskRepository.Add(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}