using MediatR;
namespace Velyo.Application.Worklogs.Queries.GetWorklogsByTask;

public record GetWorklogsByTaskQuery(Guid TaskId) : IRequest<IEnumerable<WorklogDto>>;