namespace Domain.Abstractions;

public interface IAggrerateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    IReadOnlyCollection<IImmediateDomainEvent> ImmediateEvents { get; }
    IReadOnlyCollection<IDeferredDomainEvent> DeferredEvents { get; }


    void RaiseEvent(IDomainEvent domainEvent);
    void RemoveEvent(IDomainEvent domainEvent);
    void ClearEvents();
}