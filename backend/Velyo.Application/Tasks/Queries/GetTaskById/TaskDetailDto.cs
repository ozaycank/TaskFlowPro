using Velyo.Domain.Enums;

namespace Velyo.Application.Tasks.Queries.GetTaskById;

public record TaskDetailDto(
    Guid Id,
    Guid ProjectId,
    string Title,
    string? Description,
    Guid StateId,
    PriorityLevel Priority,
    Guid? AssigneeId,
    float OrderIndex,
    DateTimeOffset? DueDate,
    DateTimeOffset CreatedAt,
    Dictionary<string, string> CustomFieldsData,
    Guid? ParentTaskId,
    List<string> Labels
);