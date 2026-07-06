using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Worklogs.Queries.GetWorklogsByTask;

public class GetWorklogsByTaskQueryHandler : IRequestHandler<GetWorklogsByTaskQuery, IEnumerable<WorklogDto>>
{
    private readonly IWorklogRepository _worklogRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetWorklogsByTaskQueryHandler(IWorklogRepository worklogRepository, ITaskItemRepository taskRepository, IWorkspaceAuthorizationService authService)
    {
        _worklogRepository = worklogRepository;
        _taskRepository = taskRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<WorklogDto>> Handle(GetWorklogsByTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);

        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        var worklogs = await _worklogRepository.GetByTaskIdAsync(request.TaskId, cancellationToken);

        return worklogs.Select(w => new WorklogDto(w.Id, w.TaskId, w.UserId, w.TimeSpentSeconds, w.StartDate, w.Description, w.CreatedAt));
    }
}