namespace Velyo.Application.ActivityLogs.Queries.GetTaskActivity;

public record ActivityLogDto(
    Guid Id,
    Guid UserId,
    string Action,
    string Details,
    DateTimeOffset CreatedAt);