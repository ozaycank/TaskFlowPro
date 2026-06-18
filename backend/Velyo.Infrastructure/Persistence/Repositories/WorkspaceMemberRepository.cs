using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

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

    // YENİ EKLENEN METOT: Tekil üyeyi bulmak için kullanılır. (Authorization için çok kritik)
    public async Task<WorkspaceMember?> GetMemberAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkspaceMembers
            .FirstOrDefaultAsync(wm => wm.WorkspaceId == workspaceId && wm.UserId == userId, cancellationToken);
    }

    public void Add(WorkspaceMember workspaceMember)
    {
        _context.WorkspaceMembers.Add(workspaceMember);
    }

    public void Remove(WorkspaceMember member)
    {
        _context.WorkspaceMembers.Remove(member);
    }
    public async Task<IEnumerable<WorkspaceMember>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkspaceMembers
            .Where(wm => wm.UserId == userId)
            .ToListAsync(cancellationToken);
    }
    public void Update(WorkspaceMember workspaceMember)
    {
        _context.WorkspaceMembers.Update(workspaceMember);
    }

    public void Delete(WorkspaceMember workspaceMember)
    {
        _context.WorkspaceMembers.Remove(workspaceMember);
    }

    public async Task<bool> IsUserMemberAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkspaceMembers
            .AnyAsync(wm => wm.WorkspaceId == workspaceId && wm.UserId == userId, cancellationToken);
    }
}