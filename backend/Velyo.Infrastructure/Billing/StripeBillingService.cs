using Microsoft.Extensions.Configuration;
using Stripe.Checkout;
using Stripe.BillingPortal; // Customer Portal için eklendi
using Velyo.Application.Common.Interfaces.Services;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace Velyo.Infrastructure.Billing;

public class StripeBillingService : IBillingService
{
    private readonly string _successUrl;
    private readonly string _cancelUrl;

    public StripeBillingService(IConfiguration configuration)
    {
        _successUrl = configuration["Stripe:SuccessUrl"] ?? "http://localhost:3000/billing/success";
        _cancelUrl = configuration["Stripe:CancelUrl"] ?? "http://localhost:3000/billing/cancel";
    }

    public async Task<string> CreateCheckoutSessionAsync(Guid workspaceId, string stripeCustomerId, string priceId, string successUrl, string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            Customer = string.IsNullOrEmpty(stripeCustomerId) ? null : stripeCustomerId, // Müşteri yoksa Stripe yeni oluşturur
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions { Price = priceId, Quantity = 1 }
            },
            Mode = "subscription",
            SuccessUrl = successUrl ?? _successUrl,
            CancelUrl = cancelUrl ?? _cancelUrl,
            ClientReferenceId = workspaceId.ToString() // Webhook'ta hangi Workspace olduğunu anlamak için çok kritik
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return session.Url;
    }
    // Kullanıcının kredi kartını güncellemesi veya faturasını indirmesi için Stripe Müşteri Portalı
    public async Task<string> CreateCustomerPortalSessionAsync(string stripeCustomerId, string returnUrl)
    {
        var options = new Stripe.BillingPortal.SessionCreateOptions
        {
            Customer = stripeCustomerId,
            ReturnUrl = returnUrl ?? _cancelUrl
        };

        var service = new Stripe.BillingPortal.SessionService();
        var session = await service.CreateAsync(options);
        return session.Url;
    }
}