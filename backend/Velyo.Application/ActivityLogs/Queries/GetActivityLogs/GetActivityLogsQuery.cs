using MediatR;

namespace Velyo.Application.ActivityLogs.Queries.GetActivityLogs;

public record GetActivityLogsQuery(
    Guid? WorkspaceId,
    Guid? ProjectId,
    Guid? TaskId,
    Guid? UserId,
    int Limit = 50) : IRequest<IEnumerable<ActivityLogDto>>;