using MediatR;
using Stripe; // Required to construct Stripe Event
using Velyo.Application.Billing.Commands.HandleStripeWebhook;

namespace Velyo.Api.Endpoints;

public static class BillingEndpoints
{
    public static RouteGroupBuilder MapBillingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/billing").WithTags("Billing");

        // Webhook MUST be anonymous so Stripe servers can reach it
        group.MapPost("/webhook", async (HttpRequest req, IMediator mediator, IConfiguration config) =>
        {
            var json = await new StreamReader(req.Body).ReadToEndAsync();
            var endpointSecret = config["Stripe:WebhookSecret"];

            try
            {
                // CRITICAL SECURITY: Verify the request actually came from Stripe
                var stripeEvent = EventUtility.ConstructEvent(json, req.Headers["Stripe-Signature"], endpointSecret);

                // Using the exact string literal from Stripe's API payload avoids SDK versioning 
                // and namespace resolution conflicts. It's the most robust way to handle webhooks.
                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    if (session != null && !string.IsNullOrEmpty(session.ClientReferenceId))
                    {
                        await mediator.Send(new HandleStripeWebhookCommand(
                            stripeEvent.Type,
                            session.ClientReferenceId, // Workspace ID
                            session.CustomerId,
                            session.SubscriptionId,
                            "active"));
                    }
                }

                // can add more event types (invoice.paid, invoice.payment_failed, etc.)
                // Example: else if (stripeEvent.Type == "invoice.payment_failed") { ... }

                return Results.Ok();
            }
            catch (StripeException e)
            {
                return Results.BadRequest(e.Message);
            }
        })
        .WithName("StripeWebhook")
        .WithOpenApi()
        .AllowAnonymous();

        return group;
    }
}