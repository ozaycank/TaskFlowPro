using Microsoft.EntityFrameworkCore;
using TaskFlowPro.Application.Common.Interfaces.Repositories;
using TaskFlowPro.Domain.Entities;

namespace TaskFlowPro.Infrastructure.Persistence.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly ApplicationDbContext _context;

    public WorkspaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Workspace?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Workspace>> GetUserWorkspacesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkspaceMembers
            .Where(wm => wm.UserId == userId)
            .Join(_context.Workspaces, 
                  wm => wm.WorkspaceId, 
                  w => w.Id, 
                  (wm, w) => w)
            .ToListAsync(cancellationToken);
    }

    public void Add(Workspace workspace)
    {
        _context.Workspaces.Add(workspace);
    }

    public void Update(Workspace workspace)
    {
        _context.Workspaces.Update(workspace);
    }

    public void Delete(Workspace workspace)
    {
        _context.Workspaces.Remove(workspace);
    }
}