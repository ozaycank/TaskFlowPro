using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Events;

public class WorkspaceSubscriptionUpdatedEvent : DomainEvent
{
    public Guid WorkspaceId { get; }
    public Guid NewPlanId { get; }
    public SubscriptionStatus Status { get; }

    public WorkspaceSubscriptionUpdatedEvent(Guid workspaceId, Guid newPlanId, SubscriptionStatus status)
    {
        WorkspaceId = workspaceId;
        NewPlanId = newPlanId;
        Status = status;
    }
}