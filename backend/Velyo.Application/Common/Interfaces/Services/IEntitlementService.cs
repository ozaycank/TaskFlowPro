using Velyo.Domain.Enums;

namespace Velyo.Application.Common.Interfaces.Services;

public interface IEntitlementService
{
    Task<bool> HasAccessAsync(Guid workspaceId, EntitlementFeature feature, CancellationToken cancellationToken = default);
    Task EnsureQuotaNotExceededAsync(Guid workspaceId, EntitlementFeature feature, int currentUsage, CancellationToken cancellationToken = default);
}