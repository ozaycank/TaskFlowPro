using MediatR;
using TaskFlowPro.Domain.Enums;

namespace TaskFlowPro.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    Guid WorkspaceId,
    Guid ProjectId,
    string Title,
    string? Description,
    PriorityLevel Priority,
    float OrderIndex) : IRequest<Guid>;