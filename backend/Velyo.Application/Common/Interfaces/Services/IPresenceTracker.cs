namespace Velyo.Application.Common.Interfaces.Services;

public interface IPresenceTracker
{
    Task UserConnected(Guid userId, string connectionId);
    Task UserDisconnected(Guid userId, string connectionId);
    Task<string[]> GetOnlineUsers(Guid workspaceId);
}