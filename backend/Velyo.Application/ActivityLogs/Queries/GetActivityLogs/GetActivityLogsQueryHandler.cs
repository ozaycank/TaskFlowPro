using MediatR;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Application.ActivityLogs.Queries.GetActivityLogs;

public class GetActivityLogsQueryHandler : IRequestHandler<GetActivityLogsQuery, IEnumerable<ActivityLogDto>>
{
    private readonly IActivityLogRepository _repository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetActivityLogsQueryHandler(IActivityLogRepository repository, IWorkspaceAuthorizationService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<IEnumerable<ActivityLogDto>> Handle(GetActivityLogsQuery request, CancellationToken cancellationToken)
    {
        // SECURE: Enforce authorization based on the scope requested
        if (request.WorkspaceId.HasValue)
            await _authService.AuthorizeMembershipAsync(request.WorkspaceId.Value, cancellationToken);

        IEnumerable<Domain.Entities.ActivityLog> logs = new List<Domain.Entities.ActivityLog>();

        if (request.TaskId.HasValue)
        {
            logs = await _repository.GetByTaskIdAsync(request.TaskId.Value, request.Limit, cancellationToken);
        }
        else if (request.ProjectId.HasValue)
        {
            logs = await _repository.GetByProjectIdAsync(request.ProjectId.Value, request.Limit, cancellationToken);
        }
        else if (request.UserId.HasValue)
        {
            logs = await _repository.GetByUserIdAsync(request.UserId.Value, request.Limit, cancellationToken);
        }
        else if (request.WorkspaceId.HasValue)
        {
            logs = await _repository.GetByWorkspaceIdAsync(request.WorkspaceId.Value, request.Limit, cancellationToken);
        }

        return logs.Select(l => new ActivityLogDto(
            l.Id, l.WorkspaceId, l.ProjectId, l.TaskId, l.UserId,
            l.EntityType, l.EntityId, l.Action, l.Details, l.CreatedAt))
            .OrderByDescending(x => x.CreatedAt);
    }
}