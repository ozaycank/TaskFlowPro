using MediatR;
namespace Velyo.Application.Analytics.Queries.GetBurndown;

public record GetBurndownQuery(Guid SprintId) : IRequest<IEnumerable<BurndownDataPointDto>>;