using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IWorkflowRepository
{
    void Add(Workflow workflow);
    void Update(Workflow workflow); // Added for saving state modifications
    Task<Workflow?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Workflow>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);

    // Gelen NewStateId'nin gerçekten bu Workspace'e ait bir akışta olup olmadığını doğrular
    Task<bool> StateExistsInWorkspaceAsync(Guid workspaceId, Guid stateId, CancellationToken cancellationToken = default);
}