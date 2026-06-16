using Microsoft.EntityFrameworkCore;
using TaskFlowPro.Application.Common.Interfaces.Repositories;
using TaskFlowPro.Domain.Entities;

namespace TaskFlowPro.Infrastructure.Persistence.Repositories;

public class WorkspaceMemberRepository : IWorkspaceMemberRepository
{
    private readonly ApplicationDbContext _context;

    public WorkspaceMemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkspaceMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.WorkspaceMembers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<WorkspaceMember>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkspaceMembers
            .Where(wm => wm.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
    }

    public void Add(WorkspaceMember workspaceMember)
    {
        _context.WorkspaceMembers.Add(workspaceMember);
    }

    public void Update(WorkspaceMember workspaceMember)
    {
        _context.WorkspaceMembers.Update(workspaceMember);
    }

    public void Delete(WorkspaceMember workspaceMember)
    {
        _context.WorkspaceMembers.Remove(workspaceMember);
    }
}