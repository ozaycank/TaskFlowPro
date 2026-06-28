using MediatR;

namespace Velyo.Application.Sprints.Queries.GetSprintsByProject;

public record GetSprintsByProjectQuery(Guid ProjectId) : IRequest<IEnumerable<SprintDto>>;