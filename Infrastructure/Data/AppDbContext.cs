using Domain.Entities;
using Domain.Entities.PurchaseOrder;
using Infrastructure.Data.Interceptors;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly AuditableEntityInterceptor _auditableEntityInterceptor;
    private readonly DomainEventsInterceptor    _domainEventsInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options, AuditableEntityInterceptor auditableEntityInterceptor,
        DomainEventsInterceptor domainEventsInterceptor) : base(options)
    {
        _auditableEntityInterceptor = auditableEntityInterceptor;
        _domainEventsInterceptor    = domainEventsInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntityInterceptor, _domainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
    public DbSet<OutBoxMessage> OutBoxMessages { get; set; }
}