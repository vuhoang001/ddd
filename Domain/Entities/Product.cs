using Domain.Abstractions;
using Domain.Events.Product;

namespace Domain.Entities;

public class Product : AggrerateRoot
{
    public int Id { get; private set; }
    public string ProductName { get; private set; } = null!;
    public string? ProductDescription { get; private set; }
    public int Quantity { get; private set; }

    public Product()
    {
    }

    public Product(string productName, string? productDescription, int quantity)
    {
        ProductName        = productName;
        ProductDescription = productDescription;
        Quantity           = quantity;
        RaiseEvent(new CreateProductEvent(ProductName));
    }

    public void Update(string productName, string? productDescription, int quantity)
    {
        ProductName        = productName;
        ProductDescription = productDescription;
        Quantity           = quantity;
    }
}