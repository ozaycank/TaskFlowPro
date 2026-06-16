using MediatR;

namespace TaskFlowPro.Application.Tasks.Queries.GetTasksByProject;

public record GetTasksByProjectQuery(Guid ProjectId) : IRequest<IEnumerable<TaskDto>>;