using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Analytics.Queries.GetBurndown;

public class GetBurndownQueryHandler : IRequestHandler<GetBurndownQuery, IEnumerable<BurndownDataPointDto>>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetBurndownQueryHandler(ISprintRepository sprintRepository, ITaskItemRepository taskRepository, IProjectRepository projectRepository, IWorkspaceAuthorizationService authService)
    {
        _sprintRepository = sprintRepository;
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<BurndownDataPointDto>> Handle(GetBurndownQuery request, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(request.SprintId, cancellationToken);
        if (sprint == null || !sprint.StartDate.HasValue || !sprint.EndDate.HasValue)
            throw new NotFoundException(nameof(Sprint), request.SprintId);

        var project = await _projectRepository.GetByIdAsync(sprint.ProjectId, cancellationToken);
        if (project == null) throw new NotFoundException(nameof(Project), sprint.ProjectId);

        await _authService.AuthorizeMembershipAsync(project.WorkspaceId, cancellationToken);

        var tasks = await _taskRepository.GetBySprintIdAsync(request.SprintId, cancellationToken);
        var totalTasks = tasks.Count();

        var totalDays = (sprint.EndDate.Value - sprint.StartDate.Value).Days;
        if (totalDays <= 0) totalDays = 1;

        var result = new List<BurndownDataPointDto>();
        decimal burnRate = (decimal)totalTasks / totalDays;

        // Gelecek fazlar için deterministik bir mock projeksiyon.
        // Gerçek bir burndown için günlük task snapshot'ları ActivityLog üzerinden çekilir.
        for (int i = 0; i <= totalDays; i++)
        {
            var date = sprint.StartDate.Value.AddDays(i);
            var ideal = totalTasks - (burnRate * i);
            var remaining = totalTasks; // In a real scenario, subtract tasks completed before 'date'

            result.Add(new BurndownDataPointDto(date, remaining, Math.Max(0, ideal)));
        }

        return result;
    }
}