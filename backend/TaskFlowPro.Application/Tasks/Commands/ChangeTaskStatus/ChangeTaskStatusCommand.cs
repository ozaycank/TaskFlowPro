using MediatR;
using TaskStatus = TaskFlowPro.Domain.Enums.TaskStatus;

namespace TaskFlowPro.Application.Tasks.Commands.ChangeTaskStatus;

public record ChangeTaskStatusCommand(
    Guid TaskId,
    TaskStatus NewStatus,
    float NewOrderIndex) : IRequest;