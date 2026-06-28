using Velyo.Domain.Enums;

namespace Velyo.Application.Sprints.Queries.GetSprintsByProject;

public record SprintDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    string? Goal,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    SprintStatus Status,
    DateTimeOffset CreatedAt);