using Velyo.Domain.Common.Models; // Eksik referans eklendi

namespace Velyo.Domain.Common.Interfaces;

public interface IHasDomainEvents
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}