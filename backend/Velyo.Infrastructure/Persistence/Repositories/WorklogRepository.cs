using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class WorklogRepository : IWorklogRepository
{
    private readonly ApplicationDbContext _context;

    public WorklogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Worklog worklog)
    {
        _context.Worklogs.Add(worklog);
    }

    public void Update(Worklog worklog)
    {
        _context.Worklogs.Update(worklog);
    }

    public void Delete(Worklog worklog)
    {
        _context.Worklogs.Remove(worklog);
    }

    public async Task<Worklog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Worklogs
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Worklog>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        return await _context.Worklogs
            .Where(w => w.TaskId == taskId)
            .OrderByDescending(w => w.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Worklog>> GetByWorkspaceAndDateRangeAsync(Guid workspaceId, DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Worklogs
            .Where(w => w.WorkspaceId == workspaceId && w.StartDate >= startDate && w.StartDate <= endDate)
            .OrderBy(w => w.StartDate)
            .ToListAsync(cancellationToken);
    }
}