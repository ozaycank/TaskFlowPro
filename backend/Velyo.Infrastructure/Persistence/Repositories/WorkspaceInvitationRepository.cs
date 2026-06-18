using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class WorkspaceInvitationRepository : IWorkspaceInvitationRepository
{
    private readonly ApplicationDbContext _context;

    public WorkspaceInvitationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(WorkspaceInvitation invitation)
    {
        _context.WorkspaceInvitations.Add(invitation);
    }

    public async Task<WorkspaceInvitation?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.WorkspaceInvitations
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public async Task<WorkspaceInvitation?> GetPendingInvitationAsync(Guid workspaceId, string email, CancellationToken cancellationToken = default)
    {
        return await _context.WorkspaceInvitations
            .FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId
                                   && x.Email == email
                                   && x.Status == Domain.Enums.InvitationStatus.Pending,
                                 cancellationToken);
    }
}