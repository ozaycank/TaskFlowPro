using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Projects.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Project>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Where(p => p.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
    }

    public void Add(Project project)
    {
        _context.Projects.Add(project);
    }

    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    public void Delete(Project project)
    {
        _context.Projects.Remove(project);
    }
}