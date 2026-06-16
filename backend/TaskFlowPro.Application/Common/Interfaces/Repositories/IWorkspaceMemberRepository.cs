using TaskFlowPro.Domain.Entities;

namespace TaskFlowPro.Application.Common.Interfaces.Repositories;

public interface IWorkspaceMemberRepository
{
    Task<WorkspaceMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkspaceMember>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    void Add(WorkspaceMember workspaceMember);
    void Update(WorkspaceMember workspaceMember);
    void Delete(WorkspaceMember workspaceMember);
}