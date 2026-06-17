using MediatR;
using Velyo.Application.Common.Interfaces.Repositories;

namespace Velyo.Application.Tasks.Queries.GetTasksByProject;

public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskItemRepository _taskRepository;

    public GetTasksByProjectQueryHandler(ITaskItemRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        // GetByProjectIdAsync already orders by OrderIndex ASC (defined in Phase 4)
        var tasks = await _taskRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);

        return tasks.Select(t => new TaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.Status,
            t.Priority,
            t.AssigneeId,
            t.OrderIndex,
            t.CreatedAt
        ));
    }
}