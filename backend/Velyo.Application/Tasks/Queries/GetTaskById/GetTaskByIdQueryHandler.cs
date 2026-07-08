using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDetailDto>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetTaskByIdQueryHandler(ITaskItemRepository taskRepository, IWorkspaceAuthorizationService authService)
    {
        _taskRepository = taskRepository;
        _authService = authService;
    }

    public async Task<TaskDetailDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);

        // SECURE: Tenant Isolation
        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        return new TaskDetailDto(
            task.Id,
            task.ProjectId,
            task.Title,
            task.Description,
            task.StateId,
            task.Priority,
            task.AssigneeId,
            task.OrderIndex,
            task.DueDate,
            task.CreatedAt,
            task.CustomFieldsData.ToDictionary(cf => cf.Key, cf => cf.Value)
        );
    }
}