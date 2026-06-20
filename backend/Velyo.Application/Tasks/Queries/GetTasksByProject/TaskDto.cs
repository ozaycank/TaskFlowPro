using Velyo.Domain.Enums;
// DİKKAT: using TaskStatus = Velyo.Domain.Enums.TaskStatus; SİLİNMELİ

namespace Velyo.Application.Tasks.Queries.GetTasksByProject;

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    Guid StateId,
    PriorityLevel Priority,
    Guid? AssigneeId,
    float OrderIndex,
    DateTimeOffset CreatedAt);