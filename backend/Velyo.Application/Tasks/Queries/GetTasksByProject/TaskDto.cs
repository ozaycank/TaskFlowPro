using Velyo.Domain.Enums;
using TaskStatus = Velyo.Domain.Enums.TaskStatus;

namespace Velyo.Application.Tasks.Queries.GetTasksByProject;

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskStatus Status,
    PriorityLevel Priority,
    Guid? AssigneeId,
    float OrderIndex,
    DateTimeOffset CreatedAt);