using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Infrastructure.RealTime.Presence;

// In a real multi-node production environment, this MUST be backed by Redis.
// For now, a thread-safe singleton dictionary simulates the logic.
public class PresenceTracker : IPresenceTracker
{
    private static readonly Dictionary<Guid, List<string>> OnlineUsers = new();

    public Task UserConnected(Guid userId, string connectionId)
    {
        lock (OnlineUsers)
        {
            if (!OnlineUsers.ContainsKey(userId))
            {
                OnlineUsers[userId] = new List<string>();
            }
            OnlineUsers[userId].Add(connectionId);
        }
        return Task.CompletedTask;
    }

    public Task UserDisconnected(Guid userId, string connectionId)
    {
        lock (OnlineUsers)
        {
            if (!OnlineUsers.ContainsKey(userId)) return Task.CompletedTask;

            OnlineUsers[userId].Remove(connectionId);

            if (OnlineUsers[userId].Count == 0)
            {
                OnlineUsers.Remove(userId);
            }
        }
        return Task.CompletedTask;
    }

    public Task<string[]> GetOnlineUsers(Guid workspaceId)
    {
        lock (OnlineUsers)
        {
            // For now, returns all global online users. 
            // In a complete implementation, this intersects with Workspace members.
            return Task.FromResult(OnlineUsers.Keys.Select(k => k.ToString()).ToArray());
        }
    }
}