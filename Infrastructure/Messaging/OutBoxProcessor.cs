using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging;

using Microsoft.Extensions.Hosting;

public sealed class OutBoxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory     _scopeFactory;
    private readonly ILogger<OutBoxProcessor> _log;
    private readonly TimeSpan                 _period = TimeSpan.FromSeconds(10);

    public OutBoxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger<OutBoxProcessor> log)
    {
        _scopeFactory = scopeFactory;
        _log          = log;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_period);
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ProcessBatch(stoppingToken);
        }

        _log.LogInformation("OutboxProcessor stopped");
    }

    private async Task ProcessBatch(CancellationToken ct)
    {
        using var scope     = _scopeFactory.CreateScope();
        var       dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var       mediator  = scope.ServiceProvider.GetRequiredService<IMediator>();

        var messages = await dbContext.OutBoxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken: ct);


        foreach (var message in messages)
        {
            try
            {
                var domainEvent = DeserialDomainEvent(message);
                if (domainEvent is not null)
                {
                    await mediator.Publish(domainEvent, ct);
                    message.ProcessedAt = DateTime.UtcNow;
                }
            }
            catch (Exception e)
            {
                message.Error = e.Message;
            }
        }

        await dbContext.SaveChangesAsync(ct);
    }

    private IDomainEvent? DeserialDomainEvent(OutBoxMessage outBoxMessage)
    {
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == outBoxMessage.Type);

        if (type is null) return null;

        return JsonSerializer.Deserialize(outBoxMessage.Payload, type) as IDomainEvent;
    }
}