using MediatR;

namespace Velyo.Application.Sprints.Commands.UpdateSprint;

public record UpdateSprintCommand(
    Guid SprintId,
    string Name,
    string? Goal,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate) : IRequest;