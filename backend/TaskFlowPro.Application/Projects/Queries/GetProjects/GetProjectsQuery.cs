using MediatR;

namespace TaskFlowPro.Application.Projects.Queries.GetProjects;

public record GetProjectsQuery(Guid WorkspaceId) : IRequest<IEnumerable<ProjectDto>>;