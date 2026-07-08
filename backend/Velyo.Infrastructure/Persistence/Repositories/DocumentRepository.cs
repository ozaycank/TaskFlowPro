using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly ApplicationDbContext _context;

    public DocumentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Document document) => _context.Documents.Add(document);
    public void Update(Document document) => _context.Documents.Update(document);
    public void Delete(Document document) => _context.Documents.Remove(document);

    public async Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Documents.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Document>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        return await _context.Documents
            .Where(d => d.WorkspaceId == workspaceId && d.ProjectId == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Document>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _context.Documents
            .Where(d => d.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasChildrenAsync(Guid documentId, CancellationToken cancellationToken = default)
    {
        return await _context.Documents.AnyAsync(d => d.ParentDocumentId == documentId, cancellationToken);
    }
}