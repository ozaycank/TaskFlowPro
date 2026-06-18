using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Infrastructure.RealTime.Constants;

namespace Velyo.Infrastructure.RealTime;

[Authorize] // SAAS SECURITY: Sadece giriş yapmış kullanıcılar sokete bağlanabilir
public class VelyoHub : Hub
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;

    public VelyoHub(
        ICurrentUserService currentUserService,
        IWorkspaceMemberRepository workspaceMemberRepository)
    {
        _currentUserService = currentUserService;
        _workspaceMemberRepository = workspaceMemberRepository;
    }

    public override async Task OnConnectedAsync()
    {
        if (Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            // Kullanıcının aktif olduğu tüm workspace'leri bul ve o odalara (groups) ekle.
            // Bu sayede o workspace'te olan olayları anında duyabilecek.
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
        // Grup çıkışları SignalR tarafından otomatik yönetilir (Connection koptuğunda).
        // İleride "Online/Offline" presence tracking buraya eklenecektir.
        await base.OnDisconnectedAsync(exception);
    }
}