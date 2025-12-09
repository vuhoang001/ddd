using Domain.Abstractions;
using Domain.ValueObject;

namespace Domain.Entities.PurchaseOrder;

public class PurchaseOrder : AggrerateRoot
{
    public int Id { get; private set; }
    public string DocCode { get; private set; } = null!;
    public string DocName { get; private set; } = null!;

    private readonly List<PurchaseOrderLine> _purchaseOrderLines;
    public IReadOnlyList<PurchaseOrderLine> PurchaseOrderLines => _purchaseOrderLines;

    private PurchaseOrder()
    {
        _purchaseOrderLines = [];
    }

    public PurchaseOrder(string docCode, string docName)
    {
        DocCode             = docCode;
        DocName             = docName;
        _purchaseOrderLines = [];
    }

    public void AddLine(int productId, string productCode, string productName, Quantity quantity, Price unitPrice)
    {
        var newPurchaesOrderLine = new PurchaseOrderLine(
            purchaseOrderId: Id,
            productId: productId,
            productCode: productCode,
            productName: productName,
            quantity: quantity,
            unitPrice: unitPrice
        );

        _purchaseOrderLines.Add(newPurchaesOrderLine);
    }

    public void RemoveLine(int lineId)
    {
        var existedLine = _purchaseOrderLines.FirstOrDefault(l => l.Id == lineId);
        if (existedLine is not null) _purchaseOrderLines.Remove(existedLine);
    }
}