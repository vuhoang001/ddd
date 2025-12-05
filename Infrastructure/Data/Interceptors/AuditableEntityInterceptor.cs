using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuditableEntityInterceptor(IServiceProvider serviceProvider)
    {
        _dateTimeProvider = serviceProvider.GetRequiredService<IDateTimeProvider>();
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }


    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        UpdateAuditEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    private void UpdateAuditEntities(DbContext? context)
    {
        if (context is null) return;

        var entries = context.ChangeTracker.Entries<IAuditableEntity>();

        var now = _dateTimeProvider.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.SetCreatedAt(now);

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.SetUpdateAt(now);
                entry.Property(e => e.CreatedAt).IsModified = false;
            }
        }
    }
}