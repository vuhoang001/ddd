using Domain.ValueObject;

namespace Domain.Entities.PurchaseOrder;

public class PurchaseOrderLine
{
    public int Id { get; private set; }
    public int PurchaseOrderId { get; private set; }
    public int ProductId { get; private set; }
    public string ProductCode { get; private set; } = null!;
    public string ProductName { get; private set; } = null!;
    public Quantity Quantity { get; private set; } = null!;
    public Price UnitPrice { get; private set; } = null!;

    public PurchaseOrderLine()
    {
    }

    public PurchaseOrderLine(int purchaseOrderId, int productId, string productCode, string productName,
        Quantity quantity, Price unitPrice)
    {
        PurchaseOrderId = purchaseOrderId;
        ProductId       = productId;
        ProductCode     = productCode;
        ProductName     = productName;
        Quantity        = quantity;
        UnitPrice       = unitPrice;
    }
}