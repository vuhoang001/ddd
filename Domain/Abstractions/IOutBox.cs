using Domain.Entities;

namespace Domain.Abstractions;

public interface IOutBox
{
    void Enqueue(IDomainEvent domainEvent);

    void EnqueueRange(List<IDomainEvent> domainEvents);

    Task PublishImmediateAsync(IDomainEvent @event, CancellationToken cancellationToken = default);

    Task PublishImmediateRangeAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}