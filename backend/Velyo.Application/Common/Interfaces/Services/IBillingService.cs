namespace Velyo.Application.Common.Interfaces.Services;

public interface IBillingService
{
    Task<string> CreateCheckoutSessionAsync(Guid workspaceId, string stripeCustomerId, string priceId, string successUrl, string cancelUrl);
    Task<string> CreateCustomerPortalSessionAsync(string stripeCustomerId, string returnUrl);
}