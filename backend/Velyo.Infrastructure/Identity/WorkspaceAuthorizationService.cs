using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Enums;

namespace Velyo.Infrastructure.Identity;

public class WorkspaceAuthorizationService : IWorkspaceAuthorizationService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;

    // Cache authorization checks per HTTP Request to prevent N+1 queries
    private readonly Dictionary<Guid, bool> _membershipCache = new();

    public WorkspaceAuthorizationService(
        ICurrentUserService currentUserService,
        IWorkspaceMemberRepository workspaceMemberRepository)
    {
        _currentUserService = currentUserService;
        _workspaceMemberRepository = workspaceMemberRepository;
    }

    public async Task<bool> IsMemberAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            return false; // Not authenticated
        }

        if (_membershipCache.TryGetValue(workspaceId, out var isMember))
        {
            return isMember;
        }

        isMember = await _workspaceMemberRepository.IsUserMemberAsync(workspaceId, userId, cancellationToken);
        _membershipCache[workspaceId] = isMember;

        return isMember;
    }

    public async Task<bool> HasRoleAsync(Guid workspaceId, WorkspaceRole requiredRole, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var userId))
            return false;

        var member = await _workspaceMemberRepository.GetMemberAsync(workspaceId, userId, cancellationToken);
        if (member == null) return false;

        // FIXED: Sizin Enum yapınızda sayı KÜÇÜLDÜKÇE yetki ARTIYOR (Owner=1, Admin=2).
        // Bu yüzden kullanıcının rol numarası, istenen rol numarasından KÜÇÜK VEYA EŞİT olmalıdır.
        return (int)member.Role <= (int)requiredRole;
    }

    public async Task AuthorizeMembershipAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var isAuthorized = await IsMemberAsync(workspaceId, cancellationToken);
        if (!isAuthorized)
        {
            throw new ForbiddenAccessException("You do not have permission to access or modify resources in this workspace.");
        }
    }

    public async Task AuthorizeRoleAsync(Guid workspaceId, WorkspaceRole requiredRole, CancellationToken cancellationToken = default)
    {
        var hasRole = await HasRoleAsync(workspaceId, requiredRole, cancellationToken);
        if (!hasRole)
        {
            throw new ForbiddenAccessException($"You must be an {requiredRole} to perform this action.");
        }
    }
}