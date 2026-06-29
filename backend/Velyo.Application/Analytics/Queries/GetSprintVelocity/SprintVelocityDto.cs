namespace Velyo.Application.Analytics.Queries.GetSprintVelocity;

public record SprintVelocityDto(
    Guid SprintId,
    string SprintName,
    int CompletedTasks,
    int TotalTasks);