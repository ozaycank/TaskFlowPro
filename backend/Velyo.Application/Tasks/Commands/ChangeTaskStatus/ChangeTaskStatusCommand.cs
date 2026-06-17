using MediatR;
using TaskStatus = Velyo.Domain.Enums.TaskStatus;

namespace Velyo.Application.Tasks.Commands.ChangeTaskStatus;

public record ChangeTaskStatusCommand(
    Guid TaskId,
    TaskStatus NewStatus,
    float NewOrderIndex) : IRequest;