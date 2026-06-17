using MediatR;
using Velyo.Application.Common.Interfaces.Repositories;

namespace Velyo.Application.Projects.Queries.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetByWorkspaceIdAsync(request.WorkspaceId, cancellationToken);

        return projects.Select(p => new ProjectDto(
            p.Id,
            p.WorkspaceId,
            p.Name,
            p.Description,
            p.CreatedAt
        ));
    }
}