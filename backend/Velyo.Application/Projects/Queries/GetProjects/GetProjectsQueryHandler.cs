using MediatR;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Application.Projects.Queries.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authorizationService;

    public GetProjectsQueryHandler(
        IProjectRepository projectRepository,
        IWorkspaceAuthorizationService authorizationService)
    {
        _projectRepository = projectRepository;
        _authorizationService = authorizationService;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        // 1. TENANT ISOLATION CHECK: Prevent horizontal privilege escalation
        await _authorizationService.AuthorizeMembershipAsync(request.WorkspaceId, cancellationToken);

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