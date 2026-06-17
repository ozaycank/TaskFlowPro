using Velyo.Domain.Enums;
namespace Velyo.Application.Common.Interfaces.Services;

public interface IWorkspaceAuthorizationService
{
    Task<bool> IsMemberAsync(Guid workspaceId, CancellationToken cancellationToken = default);

    // Future-proof for Phase 12 (Role-Based Authorization)
    Task<bool> HasRoleAsync(Guid workspaceId, WorkspaceRole requiredRole, CancellationToken cancellationToken = default);
    // Utility method to fail fast
    Task AuthorizeMembershipAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task AuthorizeRoleAsync(Guid workspaceId, Velyo.Domain.Enums.WorkspaceRole requiredRole, CancellationToken cancellationToken = default);
}