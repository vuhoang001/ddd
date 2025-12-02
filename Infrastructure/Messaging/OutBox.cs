using Domain.Abstractions;

namespace Infrastructure.Messaging;

public class OutBox : IOutBox
{
    public void Enqueue(IDomainEvent domainEvent)
    {
        throw new NotImplementedException();
    }

    public void EnqueueRange(List<IDomainEvent> domainEvents)
    {
        throw new NotImplementedException();
    }

    public Task PublishImmediateAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task PublishImmediateRangeAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}