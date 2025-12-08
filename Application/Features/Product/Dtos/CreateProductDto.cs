namespace Application.Features.Product.Dtos;

public class CreateProductDto
{
    public required string ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public int Quantity { get; set; }
}