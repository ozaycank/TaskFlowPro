using MediatR;
namespace Velyo.Application.Tasks.Queries.GetTaskById;

public record GetTaskByIdQuery(Guid TaskId) : IRequest<TaskDetailDto>;