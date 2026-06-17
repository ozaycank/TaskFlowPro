using MediatR;

namespace Velyo.Application.Projects.Queries.GetProjects;

public record GetProjectsQuery(Guid WorkspaceId) : IRequest<IEnumerable<ProjectDto>>;