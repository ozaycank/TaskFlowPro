using TaskFlowPro.Domain.Enums;
using TaskStatus = TaskFlowPro.Domain.Enums.TaskStatus;

namespace TaskFlowPro.Application.Tasks.Queries.GetTasksByProject;

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskStatus Status,
    PriorityLevel Priority,
    Guid? AssigneeId,
    float OrderIndex,
    DateTimeOffset CreatedAt);