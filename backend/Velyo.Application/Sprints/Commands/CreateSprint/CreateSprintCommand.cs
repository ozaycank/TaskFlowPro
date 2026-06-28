using MediatR;

namespace Velyo.Application.Sprints.Commands.CreateSprint;

public record CreateSprintCommand(
    Guid ProjectId,
    string Name,
    string? Goal,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate) : IRequest<Guid>;