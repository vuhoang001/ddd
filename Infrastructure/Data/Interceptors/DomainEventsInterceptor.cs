using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data.Interceptors;

public class DomainEventsInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        var context = eventData.Context;
        if (context is not AppDbContext dbContext)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        ConvertToDomainEventsToOutBoxMessages(dbContext);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ConvertToDomainEventsToOutBoxMessages(AppDbContext? context)
    {
        if (context is null) return;

        var entities = context.ChangeTracker
            .Entries<IAggrerateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();


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