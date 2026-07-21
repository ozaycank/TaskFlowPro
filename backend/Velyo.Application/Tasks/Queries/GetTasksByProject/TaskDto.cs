using Velyo.Domain.Enums;

namespace Velyo.Application.Tasks.Queries.GetTasksByProject;

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    Guid StateId,
    PriorityLevel Priority,
    Guid? AssigneeId,
    float OrderIndex,
    DateTimeOffset CreatedAt,
    Guid? ParentTaskId,
    List<string> Labels
);