using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Infrastructure.RealTime.Constants;

namespace Velyo.Infrastructure.RealTime;

[Authorize]
public class VelyoHub : Hub
{
    private readonly IPresenceTracker _presenceTracker;
    private readonly ICurrentUserService _currentUserService;
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;

    public VelyoHub(
        IPresenceTracker presenceTracker,
        ICurrentUserService currentUserService,
        IWorkspaceMemberRepository workspaceMemberRepository)
    {
        _presenceTracker = presenceTracker;
        _currentUserService = currentUserService;
        _workspaceMemberRepository = workspaceMemberRepository;
    }

    public override async Task OnConnectedAsync()
    {
        if (Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            await _presenceTracker.UserConnected(userId, Context.ConnectionId);

            // Broadcast to others that this user is online
            await Clients.Others.SendAsync("UserIsOnline", userId);

            // Bind user to workspace rooms
            var memberships = await _workspaceMemberRepository.GetByUserIdAsync(userId);
            foreach (var membership in memberships)
            {
                var groupName = HubGroups.WorkspaceGroup(membership.WorkspaceId);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }
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