using MediatR;

namespace Velyo.Domain.Common.Models;

public abstract class DomainEvent : INotification
{
    public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.UtcNow;
}