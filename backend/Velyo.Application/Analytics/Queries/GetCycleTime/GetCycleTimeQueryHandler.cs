using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Analytics.Queries.GetCycleTime;

public class GetCycleTimeQueryHandler : IRequestHandler<GetCycleTimeQuery, IEnumerable<CycleTimeDto>>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetCycleTimeQueryHandler(ITaskItemRepository taskRepository, IProjectRepository projectRepository, IWorkspaceAuthorizationService authService)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<CycleTimeDto>> Handle(GetCycleTimeQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null) throw new NotFoundException(nameof(Project), request.ProjectId);

        await _authService.AuthorizeMembershipAsync(project.WorkspaceId, cancellationToken);

        var tasks = await _taskRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);

        var result = new List<CycleTimeDto>();

        foreach (var task in tasks.Where(t => t.LastModifiedAt.HasValue))
        {
            var leadTime = (task.LastModifiedAt!.Value - task.CreatedAt).TotalDays;
            var cycleTime = leadTime * 0.7; // Start to Finish (mocking ~70% of lead time for architectural placeholder)

            result.Add(new CycleTimeDto(task.Id, task.Title, Math.Round(leadTime, 1), Math.Round(cycleTime, 1)));
        }

        return result;
    }
}