using MediatR;
namespace Velyo.Application.Analytics.Queries.GetSprintVelocity;

public record GetSprintVelocityQuery(Guid ProjectId) : IRequest<IEnumerable<SprintVelocityDto>>;