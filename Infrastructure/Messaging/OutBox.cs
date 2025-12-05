using Application.Interfaces;
using Domain.Abstractions;
using Infrastructure.Data;
using MediatR;

namespace Infrastructure.Messaging;

public class OutBox : IOutBox
{
    private readonly AppDbContext      _context;
    private readonly IEventSerializer  _eventSerializer;
    private readonly IMediator         _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public OutBox(AppDbContext context, IEventSerializer eventSerializer, IMediator mediator,
        IDateTimeProvider dateTimeProvider)
    {
        _context          = context;
        _eventSerializer  = eventSerializer;
        _mediator         = mediator;
        _dateTimeProvider = dateTimeProvider;
    }

    public void Enqueue(IDomainEvent domainEvent)
    {
        var now = _dateTimeProvider.UtcNow;

        _context.OutBoxMessages.Add(new OutBoxMessage
        {
            Id        = Guid.NewGuid(),
            Type      = domainEvent.GetType().AssemblyQualifiedName!,
            Payload   = _eventSerializer.Serialize(domainEvent),
            CreatedAt = now,
        });
    }

    public void EnqueueRange(List<IDomainEvent> domainEvents)
    {
        var now = _dateTimeProvider.UtcNow;
        var eventsList = domainEvents.Select(x => new OutBoxMessage
        {
            Id        = Guid.NewGuid(),
            Type      = x.GetType().AssemblyQualifiedName!,
            Payload   = _eventSerializer.Serialize(x),
            CreatedAt = now,
        });

        _context.OutBoxMessages.AddRange(eventsList);
    }

    public async Task PublishImmediateAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
    {
        if (@event is null) throw new ArgumentException(nameof(@event));
        await _mediator.Publish(@event, cancellationToken);
    }

    public async Task PublishImmediateRangeAsync(IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        if (events == null)
            throw new ArgumentNullException(nameof(events));

        var eventList = events.ToList();

        foreach (var @event in eventList)
        {
            await _mediator.Publish(@event, cancellationToken);
        }
    }
}