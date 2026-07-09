namespace Velyo.Application.ActivityLogs.Queries.GetActivityLogs;

public record ActivityLogDto(
    Guid Id,
    Guid WorkspaceId,
    Guid? ProjectId,
    Guid? TaskId,
    Guid UserId,
    string EntityType,
    Guid EntityId,
    string Action,
    string? Details,
    DateTimeOffset CreatedAt);