using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Services;

public interface ISearchService
{
    Task<IEnumerable<SearchProjection>> SearchWorkspaceAsync(Guid workspaceId, string searchTerm, int take = 10, CancellationToken cancellationToken = default);
}