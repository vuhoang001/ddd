using System.Text.Json.Serialization;

namespace Domain.Abstractions;

public class AggregateRoot : IAggregateRoot
{
    private readonly List<IImmediateDomainEvent> _immediateEvents = [];
    private readonly List<IDomainEvent>          _domainEvents    = [];
    [JsonIgnore] public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public IReadOnlyCollection<IImmediateDomainEvent> ImmediateDomainEvents => _immediateEvents.AsReadOnly();

    public IReadOnlyCollection<IDelayedDomainEvent> DelayedDomainEvents =>
        _domainEvents.OfType<IDelayedDomainEvent>().ToList().AsReadOnly();

    public void RaiseEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(nameof(domainEvent));

        if (domainEvent is IImmediateDomainEvent immediateDomainEvent)
            _immediateEvents.Add(immediateDomainEvent);
        else
            _domainEvents.Add(domainEvent);
    }

    public void RemoveEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(nameof(domainEvent));
        if (domainEvent is IImmediateDomainEvent immediateEvent)
        {
            _immediateEvents.Remove(immediateEvent);
        }

        _domainEvents.Remove(domainEvent);
    }

    public void ClearEvents()
    {
        _domainEvents.Clear();
        _immediateEvents.Clear();
    }

    protected void Raise<T>(T domainEvent) where T : IDomainEvent
    {
        RaiseEvent(domainEvent);
    }
}