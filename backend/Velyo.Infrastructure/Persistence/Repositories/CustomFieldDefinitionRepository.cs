using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class CustomFieldDefinitionRepository : ICustomFieldDefinitionRepository
{
    private readonly ApplicationDbContext _context;

    public CustomFieldDefinitionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(CustomFieldDefinition definition)
    {
        _context.Set<CustomFieldDefinition>().Add(definition);
    }

    public async Task<CustomFieldDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<CustomFieldDefinition>()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<CustomFieldDefinition>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<CustomFieldDefinition>()
            .Where(c => c.WorkspaceId == workspaceId && c.ProjectId == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CustomFieldDefinition>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<CustomFieldDefinition>()
            .Where(c => c.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public void Delete(CustomFieldDefinition definition)
    {
        _context.Set<CustomFieldDefinition>().Remove(definition);
    }
}