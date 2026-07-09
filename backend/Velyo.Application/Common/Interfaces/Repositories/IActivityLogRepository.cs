using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IActivityLogRepository
{
    void Add(ActivityLog activityLog);
    Task<IEnumerable<ActivityLog>> GetByWorkspaceIdAsync(Guid workspaceId, int limit = 50, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActivityLog>> GetByProjectIdAsync(Guid projectId, int limit = 50, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActivityLog>> GetByTaskIdAsync(Guid taskId, int limit = 50, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActivityLog>> GetByUserIdAsync(Guid userId, int limit = 50, CancellationToken cancellationToken = default);
}