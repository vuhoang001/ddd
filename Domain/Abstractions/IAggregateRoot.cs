namespace Domain.Abstractions;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    IReadOnlyCollection<IImmediateDomainEvent> ImmediateDomainEvents { get; }
    IReadOnlyCollection<IDelayedDomainEvent> DelayedDomainEvents { get; }
    void RaiseEvent(IDomainEvent domainEvent);
    void RemoveEvent(IDomainEvent domainEvent);
    void ClearEvents();
}