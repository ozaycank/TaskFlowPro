using MediatR;
namespace Velyo.Application.Analytics.Queries.GetCycleTime;

public record GetCycleTimeQuery(Guid ProjectId) : IRequest<IEnumerable<CycleTimeDto>>;