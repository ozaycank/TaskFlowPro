using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IWorklogRepository
{
    void Add(Worklog worklog);
    void Update(Worklog worklog);
    void Delete(Worklog worklog);
    Task<Worklog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Worklog>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Worklog>> GetByWorkspaceAndDateRangeAsync(Guid workspaceId, DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);
}