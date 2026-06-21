namespace Velyo.Domain.Enums;

public enum SubscriptionStatus
{
    Trialling = 10,
    Active = 20,
    PastDue = 30, // Ödeme gecikti, Workspace Read-Only olabilir
    Canceled = 40,
    Unpaid = 50
}