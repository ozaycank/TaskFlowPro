using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IWorkspaceInvitationRepository
{
    void Add(WorkspaceInvitation invitation);
    Task<WorkspaceInvitation?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<WorkspaceInvitation?> GetPendingInvitationAsync(Guid workspaceId, string email, CancellationToken cancellationToken = default);
}