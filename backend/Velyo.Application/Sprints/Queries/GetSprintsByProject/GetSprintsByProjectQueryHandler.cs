using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Sprints.Queries.GetSprintsByProject;

public class GetSprintsByProjectQueryHandler : IRequestHandler<GetSprintsByProjectQuery, IEnumerable<SprintDto>>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetSprintsByProjectQueryHandler(ISprintRepository sprintRepository, IProjectRepository projectRepository, IWorkspaceAuthorizationService authService)
    {
        _sprintRepository = sprintRepository;
        _projectRepository = projectRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<SprintDto>> Handle(GetSprintsByProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null) throw new NotFoundException(nameof(Project), request.ProjectId);

        await _authService.AuthorizeMembershipAsync(project.WorkspaceId, cancellationToken);

        var sprints = await _sprintRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);

        return sprints.Select(s => new SprintDto(s.Id, s.ProjectId, s.Name, s.Goal, s.StartDate, s.EndDate, s.Status, s.CreatedAt));
    }
}