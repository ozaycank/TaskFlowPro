using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Entities;

public class SubscriptionPlan : Entity
{
    public PlanType Type { get; private set; }
    public string StripePriceId { get; private set; } = null!; // Stripe'taki ürün/fiyat kimliği
    public string Name { get; private set; } = null!;

    // Key: EntitlementFeature, Value: Limit (-1 means unlimited, 0 means disabled, >0 means limit)
    public Dictionary<string, int> Features { get; private set; } = new();

    protected SubscriptionPlan() { }
}