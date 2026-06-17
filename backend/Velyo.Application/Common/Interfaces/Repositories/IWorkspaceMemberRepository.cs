using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IWorkspaceMemberRepository
{
    Task<WorkspaceMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkspaceMember>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<Velyo.Domain.Entities.WorkspaceMember?> GetMemberAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsUserMemberAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    void Update(WorkspaceMember workspaceMember);
    void Add(Domain.Entities.WorkspaceMember member);
    void Delete(WorkspaceMember workspaceMember);
    void Remove(Domain.Entities.WorkspaceMember member);
}