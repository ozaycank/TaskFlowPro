using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Tasks.Queries.GetTasksByProject;

public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authorizationService;

    public GetTasksByProjectQueryHandler(
        ITaskItemRepository taskRepository,
        IProjectRepository projectRepository,
        IWorkspaceAuthorizationService authorizationService)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _authorizationService = authorizationService;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        // Fetch project to determine its WorkspaceId
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        // 1. TENANT ISOLATION CHECK: Ensure user has access to the project's workspace
        await _authorizationService.AuthorizeMembershipAsync(project.WorkspaceId, cancellationToken);

        // GetByProjectIdAsync already orders by OrderIndex ASC (defined in Phase 4)
        var tasks = await _taskRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);

        return tasks.Select(t => new TaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.Status,
            t.Priority,
            t.AssigneeId,
            t.OrderIndex,
            t.CreatedAt
        ));
    }
}