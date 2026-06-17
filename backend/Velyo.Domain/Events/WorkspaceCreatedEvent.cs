using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class WorkspaceCreatedEvent : DomainEvent
{
    public Workspace Workspace { get; }
    public Guid InitiatedByUserId { get; }

    public WorkspaceCreatedEvent(Workspace workspace, Guid initiatedByUserId)
    {
        Workspace = workspace;
        InitiatedByUserId = initiatedByUserId;
    }
}