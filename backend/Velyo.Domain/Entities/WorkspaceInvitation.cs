using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Entities;

public class WorkspaceInvitation : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public string Email { get; private set; } = null!;
    public string Token { get; private set; } = null!;
    public WorkspaceRole Role { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }

    protected WorkspaceInvitation() { }

    private WorkspaceInvitation(Guid workspaceId, string email, WorkspaceRole role, DateTimeOffset expiresAt)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        Email = email.ToLowerInvariant();
        Token = Guid.NewGuid().ToString("N"); // Secure random token
        Role = role;
        Status = InvitationStatus.Pending;
        ExpiresAt = expiresAt;
    }

    public static WorkspaceInvitation Create(Guid workspaceId, string email, WorkspaceRole role, int expiryDays = 7)
    {
        return new WorkspaceInvitation(workspaceId, email, role, DateTimeOffset.UtcNow.AddDays(expiryDays));
    }

    public void Accept()
    {
        if (Status != InvitationStatus.Pending || ExpiresAt < DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Invitation is either expired or no longer pending.");

        Status = InvitationStatus.Accepted;
    }

    public void Decline()
    {
        if (Status != InvitationStatus.Pending)
            throw new InvalidOperationException("Only pending invitations can be declined.");

        Status = InvitationStatus.Declined;
    }
}