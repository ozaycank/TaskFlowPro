namespace Velyo.Application.Worklogs.Queries.GetWorklogsByTask;

public record WorklogDto(
    Guid Id,
    Guid TaskId,
    Guid UserId,
    long TimeSpentSeconds,
    DateTimeOffset StartDate,
    string? Description,
    DateTimeOffset CreatedAt);