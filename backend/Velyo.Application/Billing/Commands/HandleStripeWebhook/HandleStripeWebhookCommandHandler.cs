using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Billing.Commands.HandleStripeWebhook;

public record HandleStripeWebhookCommand(string EventType, string ClientReferenceId, string CustomerId, string SubscriptionId, string Status) : IRequest;

public class HandleStripeWebhookCommandHandler : IRequestHandler<HandleStripeWebhookCommand>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HandleStripeWebhookCommandHandler(IWorkspaceRepository workspaceRepository, IUnitOfWork unitOfWork)
    {
        _workspaceRepository = workspaceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(HandleStripeWebhookCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.ClientReferenceId, out var workspaceId)) return;

        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId, cancellationToken);
        if (workspace == null) return;

        // Stripe'tan gelen fatura/abonelik statüsünü Velyo Domain statüsüne çevir
        SubscriptionStatus mappedStatus = request.Status switch
        {
            "active" => SubscriptionStatus.Active,
            "past_due" => SubscriptionStatus.PastDue,
            "canceled" => SubscriptionStatus.Canceled,
            "trialing" => SubscriptionStatus.Trialling,
            "unpaid" => SubscriptionStatus.Unpaid,
            _ => SubscriptionStatus.Active // Default fallback
        };

        // Note: Realistically, you would look up the PlanId from DB using StripePriceId here.
        // For architectural setup, assuming a dummy Guid for the "Pro" plan.
        Guid assumedProPlanId = Guid.Empty; // TO-DO: Fetch real plan ID

        if (request.EventType == "checkout.session.completed" || request.EventType == "invoice.paid")
        {
            // Update workspace with new status and Stripe Customer ID
            workspace.UpdateSubscription(assumedProPlanId, mappedStatus, DateTimeOffset.UtcNow.AddMonths(1));
            // You would also set workspace.StripeCustomerId here via a domain method
        }
        else if (request.EventType == "invoice.payment_failed")
        {
            workspace.UpdateSubscription(workspace.SubscriptionPlanId, SubscriptionStatus.PastDue, workspace.CurrentPeriodEnd ?? DateTimeOffset.UtcNow);
        }
        else if (request.EventType == "customer.subscription.deleted")
        {
            workspace.UpdateSubscription(workspace.SubscriptionPlanId, SubscriptionStatus.Canceled, DateTimeOffset.UtcNow);
        }

        _workspaceRepository.Update(workspace);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}