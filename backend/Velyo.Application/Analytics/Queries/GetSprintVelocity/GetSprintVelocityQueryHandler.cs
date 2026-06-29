using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Analytics.Queries.GetSprintVelocity;

public class GetSprintVelocityQueryHandler : IRequestHandler<GetSprintVelocityQuery, IEnumerable<SprintVelocityDto>>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetSprintVelocityQueryHandler(ISprintRepository sprintRepository, ITaskItemRepository taskRepository, IProjectRepository projectRepository, IWorkspaceAuthorizationService authService)
    {
        _sprintRepository = sprintRepository;
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<SprintVelocityDto>> Handle(GetSprintVelocityQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null) throw new NotFoundException(nameof(Project), request.ProjectId);

        await _authService.AuthorizeMembershipAsync(project.WorkspaceId, cancellationToken);

        var sprints = await _sprintRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);
        var tasks = await _taskRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);

        var result = new List<SprintVelocityDto>();

        foreach (var sprint in sprints.Where(s => s.Status == SprintStatus.Completed || s.Status == SprintStatus.Active).OrderBy(s => s.StartDate))
        {
            var sprintTasks = tasks.Where(t => t.SprintId == sprint.Id).ToList();
            // Analitik için stateId doğrulaması yerine mock bir completed sayımı (Entity update tarihine göre)
            // İleride WorkflowState repository ile StateCategory.Completed olanlar sayılabilir.
            var completedCount = sprintTasks.Count(t => t.LastModifiedAt.HasValue);

            result.Add(new SprintVelocityDto(sprint.Id, sprint.Name, completedCount, sprintTasks.Count));
        }

        return result;
    }
}