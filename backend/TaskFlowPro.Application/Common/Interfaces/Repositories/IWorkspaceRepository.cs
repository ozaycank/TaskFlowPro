using TaskFlowPro.Domain.Entities;

namespace TaskFlowPro.Application.Common.Interfaces.Repositories;

public interface IWorkspaceRepository
{
    Task<Workspace?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Workspace>> GetUserWorkspacesAsync(Guid userId, CancellationToken cancellationToken = default);
    void Add(Workspace workspace);
    void Update(Workspace workspace);
    void Delete(Workspace workspace);
}