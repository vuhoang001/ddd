using Domain.Abstractions;

namespace Domain.Events.Product;

public class CreateProductEvent : IImmediateDomainEvent
{
    public string ProductName { get; init; }

    public CreateProductEvent(string productName)
    {
        ProductName = productName;
    }
}