using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IWorkspaceMemberRepository
{
    Task<WorkspaceMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkspaceMember>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    void Add(Domain.Entities.WorkspaceMember member);
    Task<bool> IsUserMemberAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    void Update(WorkspaceMember workspaceMember);
    void Delete(WorkspaceMember workspaceMember);
}