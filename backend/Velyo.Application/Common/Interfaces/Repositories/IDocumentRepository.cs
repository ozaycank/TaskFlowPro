using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IDocumentRepository
{
    void Add(Document document);
    void Update(Document document);
    void Delete(Document document);
    Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Document>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Document>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<bool> HasChildrenAsync(Guid documentId, CancellationToken cancellationToken = default);
}