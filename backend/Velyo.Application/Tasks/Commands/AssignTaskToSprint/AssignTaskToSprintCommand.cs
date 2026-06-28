using MediatR;

namespace Velyo.Application.Tasks.Commands.AssignTaskToSprint;

// If SprintId is null, it moves the task to the backlog.
public record AssignTaskToSprintCommand(Guid TaskId, Guid? SprintId) : IRequest;