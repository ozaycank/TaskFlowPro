using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface ISearchProjectionRepository
{
    Task<SearchProjection?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default);
    void Add(SearchProjection projection);
    void Update(SearchProjection projection);
}