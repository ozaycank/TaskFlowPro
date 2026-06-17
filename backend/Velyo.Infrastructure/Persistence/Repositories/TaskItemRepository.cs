using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly ApplicationDbContext _context;

    public TaskItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TaskItem>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    public void Add(TaskItem taskItem)
    {
        _context.TaskItems.Add(taskItem);
    }

    public void Update(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
    }

    public void Delete(TaskItem taskItem)
    {
        _context.TaskItems.Remove(taskItem);
    }
}