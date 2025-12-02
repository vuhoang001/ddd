using Domain.Abstractions;

namespace Domain.Entities;

public class Product : AggrerateRoot
{
    public int Id { get; private set; }
    public string ProductName { get; private set; } = null!;
    public string ProductDescription { get; private set; } = null!;
    public int Quantity { get; private set; }

    private Product()
    {
    }

    public Product(string productName, string productDescription, int quantity)
    {
        ProductName        = productName;
        ProductDescription = productDescription;
        Quantity           = quantity;
    }

    public void Update(string productName, string productDescription, int quantity)
    {
        ProductName        = productName;
        ProductDescription = productDescription;
        Quantity           = quantity;
    }
}