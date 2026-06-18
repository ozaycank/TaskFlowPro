namespace Velyo.Application.Common.Interfaces.Services;

public interface IRealTimeNotifier
{
    Task SendToUserAsync<T>(Guid userId, string method, T payload, CancellationToken cancellationToken = default);
    Task SendToWorkspaceAsync<T>(Guid workspaceId, string method, T payload, CancellationToken cancellationToken = default);
    Task SendToAllAsync<T>(string method, T payload, CancellationToken cancellationToken = default);
}