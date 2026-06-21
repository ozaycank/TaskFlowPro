using Velyo.Domain.Common.Models;
using Velyo.Domain.Events;
using Velyo.Domain.Enums;
namespace Velyo.Domain.Entities;

public class Workspace : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid OwnerId { get; private set; }
    public string? StripeCustomerId { get; private set; }
    public Guid SubscriptionPlanId { get; private set; } // Default: Free Plan Id
    public SubscriptionStatus SubscriptionStatus { get; private set; }
    public DateTimeOffset? CurrentPeriodEnd { get; private set; }
    protected Workspace() { }
    public void UpdateSubscription(Guid newPlanId, SubscriptionStatus status, DateTimeOffset currentPeriodEnd)
    {
        SubscriptionPlanId = newPlanId;
        SubscriptionStatus = status;
        CurrentPeriodEnd = currentPeriodEnd;

        // Outbox'a atılıp frontend'e anında push notification (SignalR) gitmeli: "Hesabınız Pro'ya yükseltildi!"
        AddDomainEvent(new WorkspaceSubscriptionUpdatedEvent(Id, newPlanId, status));
    }
    private Workspace(string name, string? description, Guid ownerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        OwnerId = ownerId;
    }

    public static Workspace Create(string name, string? description, Guid ownerId)
    {
        var workspace = new Workspace(name, description, ownerId);

        workspace.AddDomainEvent(new WorkspaceCreatedEvent(workspace, ownerId));

        return workspace;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}