using System.Text.Json;
using Domain.Abstractions;
using Domain.Exceptions;
using Infrastructure.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data.Interceptors;

public class DomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        var context = eventData.Context;
        if (context is not AppDbContext dbContext)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        await ConvertToDomainEventsToOutBoxMessages(dbContext);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task ConvertToDomainEventsToOutBoxMessages(AppDbContext? context)
    {
        if (context is null) return;

        var entities = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var immediates = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.ImmediateDomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();


        foreach (var immediate in immediates)
        {
            foreach (var domainEvent in immediate.ImmediateDomainEvents)
            {
                try
                {
                    await mediator.Publish(domainEvent);
                }
                catch (Exception)
                {
                    throw new DomainException("Falid to publish immediate domain event.");
                }
            }

            immediate.ClearEvents();
        }


        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                var outboxMessage = new OutBoxMessage
                {
                    Type      = domainEvent.Type,
                    Payload   = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                    CreatedAt = DateTime.UtcNow
                };

                context.OutBoxMessages.Add(outboxMessage);
            }

            entity.ClearEvents();
        }
    }
}