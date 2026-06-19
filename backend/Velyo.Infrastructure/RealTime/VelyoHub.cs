using Microsoft.AspNetCore.SignalR;
using Velyo.Application.Common.Interfaces.Services;

public class VelyoHub : Hub
{
    private readonly IPresenceTracker _presenceTracker;
    private readonly ICurrentUserService _currentUserService;

    public VelyoHub(IPresenceTracker presenceTracker, ICurrentUserService currentUserService)
    {
        _presenceTracker = presenceTracker;
        _currentUserService = currentUserService;
    }

    // Constructor'a IPresenceTracker enjekte edin.
    public override async Task OnConnectedAsync()
    {
        if (Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            await _presenceTracker.UserConnected(userId, Context.ConnectionId);

            // Broadcast to others that this user is online
            await Clients.Others.SendAsync("UserIsOnline", userId);
            // ... mevcut Workspace Group ekleme kodları ...
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            await _presenceTracker.UserDisconnected(userId, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", userId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}