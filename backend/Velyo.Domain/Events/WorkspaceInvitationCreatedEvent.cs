using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class WorkspaceInvitationCreatedEvent : DomainEvent
{
    public WorkspaceInvitation Invitation { get; }
    public string WorkspaceName { get; }

    public WorkspaceInvitationCreatedEvent(WorkspaceInvitation invitation, string workspaceName)
    {
        Invitation = invitation;
        WorkspaceName = workspaceName;
    }
}