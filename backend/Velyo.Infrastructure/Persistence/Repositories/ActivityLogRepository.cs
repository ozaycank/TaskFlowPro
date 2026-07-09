using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class ActivityLogRepository : IActivityLogRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(ActivityLog activityLog)
    {
        _context.ActivityLogs.Add(activityLog);
    }

    public async Task<IEnumerable<ActivityLog>> GetByWorkspaceIdAsync(Guid workspaceId, int limit = 50, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.WorkspaceId == workspaceId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityLog>> GetByProjectIdAsync(Guid projectId, int limit = 50, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.ProjectId == projectId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityLog>> GetByTaskIdAsync(Guid taskId, int limit = 50, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.TaskId == taskId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityLog>> GetByUserIdAsync(Guid userId, int limit = 50, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}