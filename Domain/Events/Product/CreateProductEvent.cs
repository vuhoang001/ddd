using Domain.Abstractions;

namespace Domain.Events.Product;

public class CreateProductEvent : IDomainEvent
{
    public string ProductName { get; init; }

    public CreateProductEvent(string productName)
    {
        ProductName = productName;
    }
}