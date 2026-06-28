using MediatR;

namespace Velyo.Application.Sprints.Commands.StartSprint;

public record StartSprintCommand(Guid SprintId) : IRequest;