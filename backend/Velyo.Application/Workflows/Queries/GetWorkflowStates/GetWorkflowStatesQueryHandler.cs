using MediatR;
using Velyo.Application.Common.Interfaces.Repositories;

namespace Velyo.Application.Workflows.Queries.GetWorkflowStates;

public class GetWorkflowStatesQueryHandler : IRequestHandler<GetWorkflowStatesQuery, IEnumerable<WorkflowStateDto>>
{
    private readonly IWorkflowRepository _workflowRepository;

    public GetWorkflowStatesQueryHandler(IWorkflowRepository workflowRepository)
    {
        _workflowRepository = workflowRepository;
    }

    public async Task<IEnumerable<WorkflowStateDto>> Handle(GetWorkflowStatesQuery request, CancellationToken cancellationToken)
    {
        var workflows = await _workflowRepository.GetByWorkspaceIdAsync(request.WorkspaceId, cancellationToken);
        
        // Use the default workflow, or fallback to the first one available
        var activeWorkflow = workflows.FirstOrDefault(w => w.IsDefault) ?? workflows.FirstOrDefault();

        if (activeWorkflow == null)
        {
            return Enumerable.Empty<WorkflowStateDto>();
        }

        return activeWorkflow.States
            .OrderBy(s => s.OrderIndex)
            .Select(s => new WorkflowStateDto(s.Id, s.Name, s.Color, s.OrderIndex));
    }
}