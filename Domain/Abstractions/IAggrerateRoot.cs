namespace Domain.Abstractions;

public interface IAggrerateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void RaiseEvent(IDomainEvent domainEvent);
    void RemoveEvent(IDomainEvent domainEvent);
    void ClearEvents();
}