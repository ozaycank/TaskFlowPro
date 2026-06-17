using MediatR;
using Velyo.Domain.Enums;

namespace Velyo.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    Guid WorkspaceId,
    Guid ProjectId,
    string Title,
    string? Description,
    PriorityLevel Priority,
    float OrderIndex) : IRequest<Guid>;