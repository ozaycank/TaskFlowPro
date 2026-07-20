using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Enums;

namespace Velyo.Infrastructure.Identity;

public class WorkspaceAuthorizationService : IWorkspaceAuthorizationService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;

    // Cache'i userId + workspaceId birleşimiyle daha güvenli hale getiriyoruz
    private readonly Dictionary<string, bool> _membershipCache = new();

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

        // Cache anahtarını hem User hem Workspace özelinde tutuyoruz (Tenant güvenliği)
        string cacheKey = $"{userId}_{workspaceId}";

        if (_membershipCache.TryGetValue(cacheKey, out var isMember))
        {
            return isMember;
        }

        // FIX: Veritabanında kontrol et ve sonuca göre cache'le.
        isMember = await _workspaceMemberRepository.IsUserMemberAsync(workspaceId, userId, cancellationToken);
        _membershipCache[cacheKey] = isMember;

        return isMember;
    }

    public async Task<bool> HasRoleAsync(Guid workspaceId, WorkspaceRole requiredRole, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var userId))
            return false;

        var member = await _workspaceMemberRepository.GetMemberAsync(workspaceId, userId, cancellationToken);
        if (member == null) return false;

        return (int)member.Role >= (int)requiredRole;
    }

    public async Task AuthorizeMembershipAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        // FIX: Activity gibi bazı sayfalarda (örneğin global dashboard) workspaceId null gelebiliyor.
        // MediatR pipeline'ı (ValidationBehavior) devreye girmeden biz bunu boş bırakıyoruz.
        if (workspaceId == Guid.Empty)
        {
            return; // Eğer global bir query ise Workspace yetkisi aramamalıyız.
        }

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