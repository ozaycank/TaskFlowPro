using Microsoft.AspNetCore.SignalR;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Infrastructure.RealTime.Constants;

namespace Velyo.Infrastructure.RealTime;

public class SignalRNotifier : IRealTimeNotifier
{
    private readonly IHubContext<VelyoHub> _hubContext;

    public SignalRNotifier(IHubContext<VelyoHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendToUserAsync<T>(Guid userId, string method, T payload, CancellationToken cancellationToken = default)
    {
        // SignalR uses the NameIdentifier claim (UserId) as the User identifier by default
        await _hubContext.Clients.User(userId.ToString()).SendAsync(method, payload, cancellationToken);
    }

    public async Task SendToWorkspaceAsync<T>(Guid workspaceId, string method, T payload, CancellationToken cancellationToken = default)
    {
        var groupName = HubGroups.WorkspaceGroup(workspaceId);
        await _hubContext.Clients.Group(groupName).SendAsync(method, payload, cancellationToken);
    }

    public async Task SendToAllAsync<T>(string method, T payload, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.All.SendAsync(method, payload, cancellationToken);
    }
}