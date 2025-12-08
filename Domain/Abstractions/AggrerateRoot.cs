using System.Text.Json.Serialization;

namespace Domain.Abstractions;

public class AggrerateRoot : IAggrerateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];
    [JsonIgnore] public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void RaiseEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(nameof(domainEvent));
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