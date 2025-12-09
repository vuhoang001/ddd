using Domain.Entities.PurchaseOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>,
    IEntityTypeConfiguration<PurchaseOrderLine>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.HasMany(x => x.PurchaseOrderLines)
            .WithOne()
            .HasForeignKey(x => x.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
    {
        builder.Property(x => x.ProductCode)
            .IsRequired();
        builder.Property(x => x.ProductName).IsRequired();


        builder.OwnsOne(x => x.UnitPrice, price =>
        {
            price.Property(p => p.Value)
                .HasColumnName("UnitPrice")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Quantity, price =>
        {
            price.Property(p => p.Value)
                .HasColumnName("Quantity")
                .HasColumnType("int")
                .IsRequired();
        });
    }
}