using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Analytics.Queries.GetCumulativeFlow;

public class GetCumulativeFlowQueryHandler : IRequestHandler<GetCumulativeFlowQuery, IEnumerable<CumulativeFlowDataPointDto>>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetCumulativeFlowQueryHandler(ITaskItemRepository taskRepository, IProjectRepository projectRepository, IWorkflowRepository workflowRepository, IWorkspaceAuthorizationService authService)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _workflowRepository = workflowRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<CumulativeFlowDataPointDto>> Handle(GetCumulativeFlowQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null) throw new NotFoundException(nameof(Project), request.ProjectId);

        await _authService.AuthorizeMembershipAsync(project.WorkspaceId, cancellationToken);

        var tasks = await _taskRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);
        var workflows = await _workflowRepository.GetByWorkspaceIdAsync(project.WorkspaceId, cancellationToken);
        var defaultWorkflow = workflows.FirstOrDefault();

        var result = new List<CumulativeFlowDataPointDto>();
        var today = DateTimeOffset.UtcNow.Date;

        // Generate last 7 days of flow data as a projection
        for (int i = 6; i >= 0; i--)
        {
            var date = today.AddDays(-i);

            if (defaultWorkflow != null)
            {
                foreach (var state in defaultWorkflow.States)
                {
                    // Basic projection: In a real EventSourced system, we query State history.
                    var count = tasks.Count(t => t.StateId == state.Id && t.CreatedAt <= date);
                    result.Add(new CumulativeFlowDataPointDto(date, state.Name, count));
                }
            }
        }

        return result;
    }
}