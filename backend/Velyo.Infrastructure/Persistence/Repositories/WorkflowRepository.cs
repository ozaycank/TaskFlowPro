using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class WorkflowRepository : IWorkflowRepository
{
    private readonly ApplicationDbContext _context;

    public WorkflowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Workflow workflow)
    {
        _context.Set<Workflow>().Add(workflow);
    }

    // PHASE 2 ADDITION: Implement the Update method for modifying states
    public void Update(Workflow workflow)
    {
        _context.Set<Workflow>().Update(workflow);
    }

    public async Task<Workflow?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Workflow>()
            .Include(w => w.States) // State'leri de birlikte çeker
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Workflow>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Workflow>()
            .Include(w => w.States)
            .Where(w => w.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> StateExistsInWorkspaceAsync(Guid workspaceId, Guid stateId, CancellationToken cancellationToken = default)
    {
        // SECURE: Hedeflenen State'in, bu Workspace'e ait bir Workflow içinde olduğunu doğrular.
        return await _context.Set<Workflow>()
            .AnyAsync(w => w.WorkspaceId == workspaceId && w.States.Any(s => s.Id == stateId), cancellationToken);
    }
}