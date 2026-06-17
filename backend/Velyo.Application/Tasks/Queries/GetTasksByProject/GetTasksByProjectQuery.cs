using MediatR;

namespace Velyo.Application.Tasks.Queries.GetTasksByProject;

public record GetTasksByProjectQuery(Guid ProjectId) : IRequest<IEnumerable<TaskDto>>;