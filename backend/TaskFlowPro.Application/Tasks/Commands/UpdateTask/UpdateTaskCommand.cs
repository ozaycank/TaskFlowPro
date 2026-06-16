using MediatR;
using TaskFlowPro.Domain.Enums;

namespace TaskFlowPro.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(
    Guid TaskId,
    string Title,
    string? Description,
    PriorityLevel Priority) : IRequest;