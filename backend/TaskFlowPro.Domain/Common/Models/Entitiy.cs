namespace TaskFlowPro.Domain.Common.Models;

public abstract class Entity
{
    // Using Guid for scalable SaaS multi-tenancy to prevent ID guessing
    public Guid Id { get; protected set; }

    private readonly List<DomainEvent> _domainEvents = new();
    
    // Encapsulate the collection so it cannot be modified directly from the outside
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}