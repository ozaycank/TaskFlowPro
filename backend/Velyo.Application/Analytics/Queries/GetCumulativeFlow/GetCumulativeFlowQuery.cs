using MediatR;
namespace Velyo.Application.Analytics.Queries.GetCumulativeFlow;

public record GetCumulativeFlowQuery(Guid ProjectId) : IRequest<IEnumerable<CumulativeFlowDataPointDto>>;