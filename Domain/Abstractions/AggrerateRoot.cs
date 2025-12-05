namespace Domain.Abstractions;

public class AggrerateRoot : IAggrerateRoot
{
    private readonly List<IDomainEvent>          _domainEvents    = [];
    private readonly List<IImmediateDomainEvent> _immediateEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public IReadOnlyCollection<IImmediateDomainEvent> ImmediateEvents => _immediateEvents.AsReadOnly();

    public IReadOnlyCollection<IDeferredDomainEvent> DeferredEvents =>
        _domainEvents.OfType<IDeferredDomainEvent>().ToList().AsReadOnly();

    public void RaiseEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(nameof(domainEvent));

        if (domainEvent is IImmediateDomainEvent immediateDomainEvent)
            _immediateEvents.Add(immediateDomainEvent);
        _domainEvents.Add(domainEvent);
    }

    public void RemoveEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(nameof(domainEvent));

        _domainEvents.Remove(domainEvent);
    }

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }

    protected void Raise<T>(T domainEvent) where T : IDomainEvent
    {
        RaiseEvent(domainEvent);
    }
}