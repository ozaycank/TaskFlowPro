using MediatR;
using Velyo.Domain.Enums;

namespace Velyo.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(
    Guid TaskId,
    string Title,
    string? Description,
    PriorityLevel Priority,
    DateTimeOffset? DueDate,
    Guid? ParentTaskId,
    List<string>? Labels
) : IRequest;